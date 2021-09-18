using Assets.Scripts.Entities;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(Entity))]
public class EquippedItems : MonoBehaviour
{
    public Transform leftHandEquipPoint;
    public Transform rightHandEquipPoint;

    public GameObject leftHand;
    public GameObject rightHand;

    private Entity entity;

    private class HandData
    {
        public Transform equipPoint { get; set; }
        public int actionButton { get; set; }
        public KeyCode dropKey { get; set; }
    }

    void Start()
    {
        entity = gameObject.GetComponent<Entity>();
    }

    void Update()
    {
        var leftHandData = new HandData() { equipPoint = leftHandEquipPoint, actionButton = 0, dropKey = KeyCode.Q };
        var rightHandData = new HandData() { equipPoint = rightHandEquipPoint, actionButton = 1, dropKey = KeyCode.R };

        var holdableItemUnderCrosshair = GetItemAtCrosshair<IHoldable>();
        var interactableItemUnderCrosshair = GetItemAtCrosshair<IInteractable>();

        ActionForHand(ref leftHand, leftHandData, holdableItemUnderCrosshair);
        ActionForHand(ref rightHand, rightHandData, holdableItemUnderCrosshair);
        ActionForWorld(KeyCode.E, holdableItemUnderCrosshair, interactableItemUnderCrosshair);
    }

    private void ActionForHand(ref GameObject hand, HandData handData, GameObject itemAtCrosshair)
    {
        var equipPoint = handData.equipPoint;
        var actionButton = handData.actionButton;
        var dropKey = handData.dropKey;

        if (Input.GetMouseButtonDown(actionButton) && hand != null)
        {
            hand.GetComponent<IHoldable>().OnUse();
        }
        else if (Input.GetMouseButtonDown(actionButton) && hand == null && itemAtCrosshair != null)
        {
            itemAtCrosshair.transform.parent ??= equipPoint;
            itemAtCrosshair.transform.localPosition = new Vector3(0, 0, 0);
            itemAtCrosshair.transform.rotation = Quaternion.identity;

            hand = itemAtCrosshair;

            itemAtCrosshair.GetComponent<IHoldable>().OnPickup();
        }
        else if (Input.GetKeyDown(dropKey) && hand != null)
        {
            hand.transform.parent = null;
            hand.GetComponent<IHoldable>().OnDrop();
            hand = null;
        }
    }

    private void ActionForWorld(KeyCode keyCode, GameObject itemAtCrosshair, GameObject worldItemAtCrosshair)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemAtCrosshair != null)
            {
                entity.Inventory.AddItem(itemAtCrosshair.GetComponent<IItem>());
                itemAtCrosshair.SetActive(false);
            }
            else if (worldItemAtCrosshair != null)
            {
                worldItemAtCrosshair.GetComponent<IInteractable>().Interact();
            }
        }
    }

    private GameObject GetItemAtCrosshair<T>()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10.0f))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<T>() != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}