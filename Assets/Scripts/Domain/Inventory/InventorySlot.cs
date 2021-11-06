using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Items
{
    public class InventorySlot : VisualElement
    {
        public List<Item> Items;
        public Item Item;
        public int MaxItems;
        public int Amount;
        public Texture2D Icon => Item?.Details?.Icon ?? new Texture2D(1, 1);

        public InventorySlot()
        {
            Items = new List<Item>();
            Amount = 0;

            var IconTemp = new Image();
            IconTemp.image = Icon;
            Add(IconTemp);

            IconTemp.AddToClassList("slotIcon");
            AddToClassList("slotContainer");

            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        public InventorySlot(Item item, int maxItems)
        {
            Item = item;
            Items = new List<Item>() { item };
            MaxItems = maxItems;
            Amount = 1;
        }

        public void AddItem(Item item)
        {
            if (Items.Count == 0)
            {
                Item = item;
                UpdateIcon();
            }
            else if (item.Details.Name != Item.Details.Name)
            {
                return;
            }
            Items.Add(item);
        }

        public Item GetItem()
        {
            Item item = Items.FirstOrDefault();
            if (item != null)
                Items.RemoveAt(Items.IndexOf(item));
            if (Items.Count == 0)
                Item = null;
            UpdateIcon();
            return item;
        }
        public void UpdateIcon()
        {
            var IconTemp = this.Q<Image>();
            IconTemp.image = Item is null ? new Texture2D(1, 1) : Item.Details.Icon;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || Item == null)
            {
                return;
            }

            UpdateIcon();

            InventoryController.StartDrag(evt.position, this);
        }
    }
}