using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class HandController : MonoBehaviour
{
    public GameObject leftHand => entity.Equipped.SeeItemAtSlot(0)?.gameObject;
    public GameObject rightHand => entity.Equipped.SeeItemAtSlot(1)?.gameObject;

    public Entity entity;

    void Update()
    {
        var holdableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<Item>();
        var interactableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<Interactable>();

        ActionFor(Hand.Left, holdableItemUnderCrosshair);
        ActionFor(Hand.Right, holdableItemUnderCrosshair);
        InteractWithWorld(KeyCode.E, holdableItemUnderCrosshair, interactableItemUnderCrosshair);
    }

    private void ActionFor(Hand hand, GameObject itemAtCrosshair)
    {
        var handId = hand.GetIndex();
        var dropKey = hand.GetDropKey();

        var isPressingActionKey = Input.GetMouseButtonDown(handId);
        var isPressingDropKey = Input.GetKeyDown(dropKey);
        var isHoldingItem = entity.IsHoldingItemInHand(handId);

        switch (isPressingActionKey, isPressingDropKey, isHoldingItem, itemAtCrosshair)
        {
            case (true, false, true, _):
                entity.TryUseHeldItem(handId);
                break;
            case (true, false, false, null):
                entity.TryUseSkill(0);
                break;
            case (true, false, false, not null):
                entity.EquipItem(itemAtCrosshair.GetComponent<Item>(), handId);
                break;
            case (false, true, true, _):
                entity.DropItem(handId);
                break;
        }
    }

    private void InteractWithWorld(KeyCode keyCode, GameObject itemAtCrosshair, GameObject worldItemAtCrosshair)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemAtCrosshair != null)
            {
                entity.Inventory.AddItem(itemAtCrosshair.GetComponent<Item>());
                itemAtCrosshair.transform.parent = transform;
                itemAtCrosshair.SetActive(false);
            }
            else if (worldItemAtCrosshair != null)
            {
                worldItemAtCrosshair.GetComponent<Interactable>().Interact();
            }
        }
    }
}