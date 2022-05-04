using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        public int Capacity;
        public string Name;
        public List<InventorySlot> Slots { get; private set; }
        public event Action InventoryChanged;

        public void Awake()
        {
            Slots = new List<InventorySlot>();
            for (var i = 0; i < Capacity; i++)
            {
                Slots.Add(new InventorySlot());
            }
        }

        public void AddItems(List<IHoldable> items)
        {
            items.ForEach(item => AddItem(item));
        }

        public void AddItem(IHoldable item)
        {
            var preExistingSlot = Slots.FirstOrDefault(slot => slot.Item == null || slot.Item.Name == item.Name);

            if (preExistingSlot != null)
            {
                preExistingSlot.AddItem(item);
            }
            else
            {
                var slot = Slots.FirstOrDefault(s => s.Item == null);
                slot.AddItem(item);
            }

            item.OnStore(transform);
            InventoryChanged?.Invoke();
        }

        public Maybe<IHoldable> GetRandomItem()
        {
            var slot = Slots.FirstOrDefault(slot => slot.Item != null);

            if (slot != null)
            {
                var item = slot.GetItem();
                InventoryChanged?.Invoke();
                return Maybe<IHoldable>.Some(item);
            }

            return Maybe<IHoldable>.None();
        }

        public Maybe<IHoldable> GetItemAtSlot(int index)
        {
            var slot = Slots?[index];

            if (slot != null && slot.Item != null)
            {
                var item = slot.GetItem();
                InventoryChanged?.Invoke();
                return Maybe<IHoldable>.Some(item);
            }

            return Maybe<IHoldable>.None();
        }

        public Maybe<IHoldable> SeeItemAtSlot(int index)
        {
            var slot = Slots?[index];

            if (slot != null && slot.Item != null)
            {
                return Maybe<IHoldable>.Some(slot.Item);
            }

            return Maybe<IHoldable>.None();
        }

        internal void AddItemAtSlot(int index, IHoldable item)
        {
            if (index < Slots.Count && index >= 0 && Slots.ElementAtOrDefault(index) != null)
            {
                Slots[index].AddItem(item);
                item.OnStore(transform);
                InventoryChanged?.Invoke();
            }
        }
    }
}
