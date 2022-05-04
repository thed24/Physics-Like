using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class HandController : MonoBehaviour
{
    public Player player;

    public void Start()
    {
        player.Equipped.InventoryChanged += OnEquipmentChanged;
    }

    void Update()
    {
        var holdableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<IHoldable>();
        var interactableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<IInteractable>();

        ActionFor(Hand.Left, holdableItemUnderCrosshair);
        ActionFor(Hand.Right, holdableItemUnderCrosshair);
        InteractWithWorld(KeyCode.E, holdableItemUnderCrosshair, interactableItemUnderCrosshair);
    }

    private void ActionFor(Hand hand, Maybe<GameObject> itemAtCrosshair)
    {
        var handId = hand.GetIndex();
        var dropKey = hand.GetDropKey();

        var isPressingActionKey = Input.GetMouseButtonDown(handId);
        var isPressingDropKey = Input.GetKeyDown(dropKey);
        var isHoldingItem = player.IsHoldingItemInHand(handId);

        switch (isPressingActionKey, isPressingDropKey, isHoldingItem, itemAtCrosshair.HasValue)
        {
            case (true, false, true, _):
                player.UseHeldItem(handId);
                break;
            case (true, false, false, false):
                player.UseSkill(0);
                break;
            case (true, false, false, true):
                player.EquipItem(itemAtCrosshair.Value.GetComponent<IHoldable>(), handId);
                break;
            case (false, true, true, _):
                player.DropItem(handId);
                break;
        }
    }

    private void InteractWithWorld(KeyCode keyCode, Maybe<GameObject> itemAtCrosshair, Maybe<GameObject> worldItemAtCrosshair)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemAtCrosshair.HasValue)
            {
                player.Inventory.AddItem(itemAtCrosshair.Value.GetComponent<IHoldable>());
            }

            if (worldItemAtCrosshair.HasValue)
            {
                worldItemAtCrosshair.Value.GetComponent<IInteractable>().Interact();
            }
        }
    }

    private void OnEquipmentChanged()
    {
        var leftHand = player.Hands[0];
        var rightHand = player.Hands[1];

        var leftItem = player.Equipped.SeeItemAtSlot(0);
        var rightItem = player.Equipped.SeeItemAtSlot(1);

        if (leftItem.HasValue)
        {
            leftItem.Value.OnEquip(leftHand);
        }

        if (rightItem.HasValue)
        {
            rightItem.Value.OnEquip(rightHand);
        }
    }
}