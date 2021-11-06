using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class HandController : MonoBehaviour
{
    public Transform leftHandEquipPoint;
    public Transform rightHandEquipPoint;

    public GameObject leftHand => entity.Equipped.SeeItemAtSlot(0)?.gameObject;
    public GameObject rightHand => entity.Equipped.SeeItemAtSlot(1)?.gameObject;

    public Entity entity;
    
    void Start()
    {
        entity.Equipped.InventoryChanged += OnInventoryChanged;
    }

    void Update()
    {
        var holdableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<Item>();
        var interactableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<Interactable>();

        var LeftHand = new Hand() { equipPoint = leftHandEquipPoint, handId = 0, dropKey = KeyCode.Q };
        var RightHand = new Hand() { equipPoint = rightHandEquipPoint, handId = 1, dropKey = KeyCode.R };

        ActionForHand(LeftHand, holdableItemUnderCrosshair);
        ActionForHand(RightHand, holdableItemUnderCrosshair);
        ActionForWorld(KeyCode.E, holdableItemUnderCrosshair, interactableItemUnderCrosshair);
    }

    private void ActionForHand(Hand hand, GameObject itemAtCrosshair)
    {
        var equipPoint = hand.equipPoint;
        var handId = hand.handId;
        var dropKey = hand.dropKey;
        var isHoldingItem = entity.IsHoldingItemInHand(handId);

        if (Input.GetMouseButtonDown(handId) && isHoldingItem is true) // Attack
        {
            entity.TryUseHeldItem(handId);
        }
        else if (Input.GetMouseButtonDown(handId) && isHoldingItem is false && itemAtCrosshair == null){ // Skillcast
            entity.TryUseSkill(0);
        }
        else if (Input.GetMouseButtonDown(handId) && isHoldingItem is false && itemAtCrosshair != null) // Pickup
        {
            entity.EquipItem(itemAtCrosshair.GetComponent<Item>(), handId);
            HoldItemInHand(itemAtCrosshair.GetComponent<Item>(), equipPoint); // Maybe shift logic elsewhere
        }
        else if (Input.GetKeyDown(dropKey) && isHoldingItem is true) // Drop
        {
            entity.DropItem(handId);
        }
    }

    private void ActionForWorld(KeyCode keyCode, GameObject itemAtCrosshair, GameObject worldItemAtCrosshair)
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

    private void HoldItemInHand(Item item, Transform equipPoint){
        item.transform.SetParent(equipPoint);
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.transform.rotation = Quaternion.identity;
        item.GetComponent<Item>().OnPickup();
    }

    private void OnInventoryChanged()
    {
        var inventory = entity.Equipped;
        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            var item = inventory.SeeItemAtSlot(i);
            var equipPoint = i == 0 ? leftHandEquipPoint : rightHandEquipPoint;
            if (item != null)
            {
                item.gameObject.SetActive(true);
                HoldItemInHand(item, equipPoint);
            }
        }
    }

    private class Hand
    {
        public Transform equipPoint { get; set; }
        public int handId { get; set; }
        public KeyCode dropKey { get; set; }
    }
}