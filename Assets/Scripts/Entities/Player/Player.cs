using Assets.Scripts.Entities.Player;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(HudController))]
    [RequireComponent(typeof(FirstPersonController))]
    [RequireComponent(typeof(EquippedItems))]
    public class Player : Entity
    {
        private int CurrentHealth;

        void Start()
        {
            CurrentHealth = MaxHealth;
            Inventory = GetComponents<Inventory>()[1];
            Equipped = GetComponents<Inventory>()[0];
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
