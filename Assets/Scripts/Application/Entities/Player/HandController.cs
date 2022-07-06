using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FirstPersonController))]
public class HandController : MonoBehaviour
{
    public PlayerInput playerInput;
    public Player player;

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        var leftHand = playerInput.currentActionMap.FindAction("Left Hand");
        leftHand.performed += LeftHand;

        var rightHand = playerInput.currentActionMap.FindAction("Right Hand");
        rightHand.performed += RightHand;

        var dropLeftHand = playerInput.currentActionMap.FindAction("Drop Left");
        dropLeftHand.performed += DropLeftHand;

        var dropRightHand = playerInput.currentActionMap.FindAction("Drop Right");
        dropRightHand.performed += DropRightHand;

        var interact = playerInput.currentActionMap.FindAction("Interact");
        interact.performed += Interact;

        player.Equipped.InventoryChanged += OnEquipmentChanged;
    }

    public void LeftHand(InputAction.CallbackContext context) => UseHand(Hand.Left, UnityExtensions.GetItemAtCrosshair<IInteractable>());
    public void RightHand(InputAction.CallbackContext context) => UseHand(Hand.Right, UnityExtensions.GetItemAtCrosshair<IInteractable>());
    public void DropLeftHand(InputAction.CallbackContext context) => player.DropItem(Hand.Left.GetIndex());
    public void DropRightHand(InputAction.CallbackContext context) => player.DropItem(Hand.Right.GetIndex());

    public void Interact(InputAction.CallbackContext context)
    {
        var holdableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<IHoldable>();

        if (holdableItemUnderCrosshair.HasValue)
        {
            player.Inventory.AddItem(holdableItemUnderCrosshair.Value.GetComponent<IHoldable>());
        }

        var interactableItemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<IInteractable>();

        if (interactableItemUnderCrosshair.HasValue)
        {
            interactableItemUnderCrosshair.Value.GetComponent<IInteractable>().Interact();
        }
    }

    private void UseHand(Hand hand, Maybe<GameObject> itemAtCrosshair)
    {
        var handId = hand.GetIndex();
        var isHoldingItem = player.IsHoldingItemInHand(handId);

        switch (isHoldingItem, itemAtCrosshair.HasValue)
        {
            case (true, _):
                player.UseHeldItem(handId);
                break;
            case (false, false):
                player.UseSkill(0);
                break;
            case (false, true):
                player.EquipItem(itemAtCrosshair.Value.GetComponent<IHoldable>(), handId);
                break;
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