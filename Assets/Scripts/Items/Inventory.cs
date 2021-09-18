using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        private List<IItem> Items;

        public void Awake(){
            Items = new List<IItem>();
        }

        public void AddItem(IItem item){
            Items.Add(item);
            item.GameObject.SetActive(false);
        }

        public void AddItems(List<IItem> items)
        {
            Items.AddRange(items);
            items.ForEach(item => item.GameObject.SetActive(false));
        }

        public GameObject RetrieveItemAt(int index){
            if (index < Items.Count)
            {
                var item = Items[index];
                Items.RemoveAt(index);
                item.GameObject.SetActive(true);
                return item.GameObject;
            }
            return null;
        }
    }
}
