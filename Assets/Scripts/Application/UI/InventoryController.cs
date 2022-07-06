using Assets.Scripts.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    public PlayerInput playerInput;
    public List<Inventory> Inventories;
    
    private IEnumerable<VisualElement> InventoryRoots;
    private VisualElement Root;
    private static VisualElement GhostIcon;
    private static InventorySlot OriginalSlot;
    private static bool IsDragging;

    public void Start()
    {
        var toggleInventory = playerInput.currentActionMap.FindAction("Toggle Inventory");
        toggleInventory.performed += ToggleInventory;

        Root = GetComponent<UIDocument>().rootVisualElement;
        GhostIcon = Root.Query<VisualElement>("GhostIcon");
        InventoryRoots = Root.Children().First().Children();

        foreach (var inventoryRoot in InventoryRoots.Where(i => i.name != "GhostIcon"))
        {
            var matchingInventory = Inventories.First(i => i.Name.Equals(inventoryRoot.name));
            VisualElement slotContainer = inventoryRoot.Query<VisualElement>("SlotContainer");
            for (int i = 0; i < matchingInventory.Slots.Count; i++)
            {
                var slot = matchingInventory.Slots[i];
                slotContainer.Add(slot);
            }
            matchingInventory.InventoryChanged += OnInventoryChanged;
        }

        GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

        Root.visible = false;
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        Root.visible = !Root.visible;
        Time.timeScale = Root.visible ? 0 : 1;
        UnityEngine.Cursor.lockState = Root.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void StartDrag(Vector2 position, InventorySlot originalSlot)
    {
        IsDragging = true;
        OriginalSlot = originalSlot;

        GhostIcon.style.top = position.y - GhostIcon.layout.height / 2;
        GhostIcon.style.left = position.x - GhostIcon.layout.width / 2;

        GhostIcon.style.backgroundImage = originalSlot.Icon;

        GhostIcon.style.visibility = Visibility.Visible;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!IsDragging) return;

        GhostIcon.style.top = evt.position.y - GhostIcon.layout.height / 2;
        GhostIcon.style.left = evt.position.x - GhostIcon.layout.width / 2;
    }
    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!IsDragging) return;

        var inventoryAndSlots = Inventories.ToDictionary(i => i, i => i.Slots);
        var closestInventoryAndSlot = inventoryAndSlots.Select(i => i.Value.Select(s => new { Inventory = i.Key, SlotIndex = i.Key.Slots.IndexOf(s) }))
            .SelectMany(i => i)
            .Where(i => i.Inventory.Slots?[i.SlotIndex] is not null)
            .Select(i => new { Inventory = i.Inventory, SlotIndex = i.SlotIndex, Distance = Vector2.Distance(i.Inventory.Slots[i.SlotIndex].worldBound.position, GhostIcon.worldBound.position) })
            .OrderBy(i => i.Distance)
            .First();

        if (closestInventoryAndSlot.Inventory is not null && !closestInventoryAndSlot.Inventory.SeeItemAtSlot(closestInventoryAndSlot.SlotIndex).HasValue)
        {
            var item = OriginalSlot.GetItem();
            closestInventoryAndSlot.Inventory.AddItemAtSlot(closestInventoryAndSlot.SlotIndex, item);
        }
        else if (!InventoryRoots.Any(NestedRoot =>
        {
            VisualElement container = NestedRoot.Query<VisualElement>("SlotContainer");
            return GhostIcon.worldBound.Overlaps(container.worldBound);
        }))
        {
            var item = OriginalSlot.GetItem();
            item.OnDrop();
        }

        OriginalSlot.UpdateIcon();

        IsDragging = false;
        OriginalSlot = null;
        GhostIcon.style.visibility = Visibility.Hidden;
    }
    private void OnInventoryChanged()
    {
        foreach (var inventory in Inventories)
        {
            var inventoryRoot = InventoryRoots.First(i => i.name == inventory.Name);
            List<InventorySlot> slots = inventoryRoot.Query<InventorySlot>().ToList();
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = slots[i];
                if (i < inventory.Slots.Count && inventory.Slots[i].Item != null)
                {
                    slot = inventory.Slots[i];
                }
            }
        }
    }
}