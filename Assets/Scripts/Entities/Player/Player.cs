using Assets.Scripts.Entities.Player;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(GuiController))]
    [RequireComponent(typeof(FirstPersonController))]
    [RequireComponent(typeof(EquippedItems))]
    public class Player : MonoBehaviour, IEntity, IDamageable
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
        public int Dexterity { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Luck { get; set; }
        public Inventory Inventory { get; set; }
        private int CurrentHealth;

        void Start()
        {
            CurrentHealth = MaxHealth;
            Inventory = GetComponent<Inventory>();
        }

        void Update()
        {

        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }
}
