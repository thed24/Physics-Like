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

        public void Awake(){
            Slots = new List<InventorySlot>();
            for (var i = 0; i < Capacity; i++)
            {
                Slots.Add(new InventorySlot());
            }
        }

        public void AddItems(List<Item> items){
            items.ForEach(item => AddItem(item));
        }

        public void AddItem(Item item){
            var preExistingSlot = Slots.FirstOrDefault(slot => slot.Item == null || slot.Item.Details.Name == item.Details.Name);
            if (preExistingSlot != null){
                preExistingSlot.AddItem(item);
            } else {
                var slot = Slots.FirstOrDefault(s => s.Item == null);
                slot.AddItem(item);
            }
            item.gameObject.SetActive(false);
            item.gameObject.transform.SetParent(transform);
            item.gameObject.transform.localPosition = Vector3.zero;

            InventoryChanged?.Invoke();
        }

        public Item GetRandomItem(){
            var slot = Slots.FirstOrDefault(slot => slot.Item != null);
            if (slot != null){
                var item = slot.GetItem();
                InventoryChanged?.Invoke();
                return item;
            }
            return null;
        }

        public Item GetItemAtSlot(int index){
            var slot = Slots?[index];
            if (slot != null){
                var item = slot.GetItem();
                InventoryChanged?.Invoke();
                return item;
            }
            return null;
        }

        public Item SeeItemAtSlot(int index){
            if (index < Slots.Count && index >= 0 && Slots.ElementAtOrDefault(index) != null)
            {
                var slotToReturn = Slots[index];
                return slotToReturn.Item;
            }
            return null;
        }

        internal void AddItemAtSlot(int index, Item item)
        {
            if (index < Slots.Count && index >= 0 && Slots.ElementAtOrDefault(index) != null)
            {
                Slots[index].AddItem(item);
                item.gameObject.SetActive(false);
                item.gameObject.transform.SetParent(transform);
                item.gameObject.transform.localPosition = Vector3.zero;
                InventoryChanged?.Invoke();
            }
        }
    }
}
