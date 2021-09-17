using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Inventory))]
    class Entity : MonoBehaviour
    {
        [HideInInspector]
        public Inventory Inventory;
        public string Name;
        public int MaxHealth;

        public void Start(){
            Inventory = GetComponent<Inventory>();
        }
    }
}
