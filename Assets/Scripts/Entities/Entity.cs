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
        public int Level;
        public int Experience;
        public int MaxHealth;
        public int MaxMana;
        public int Dexterity;
        public int Strength;
        public int Intelligence;
        public int Luck;

        public void Start(){
            Inventory = GetComponent<Inventory>();
        }
    }
}
