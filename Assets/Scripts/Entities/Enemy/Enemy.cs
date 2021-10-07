using Assets.Scripts.Entities;
using Assets.Scripts.HoldableItems;
using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Inventory))]
    public class Enemy : Entity
    {
        public GameObject DamageText;
        private int CurrentHealth;

        void Start()
        {
            CurrentHealth = MaxHealth;
            Inventory = GetComponent<Inventory>();
        }

        void Update()
        {
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("ActiveWeapon"))
            {
                var weaponScript = collision.body.gameObject.GetComponent<Weapon>();
                var damage = weaponScript.Damage;

                TakeDamage(damage);
            }
        }

        public void TakeDamage(int damage)
        {
            CreateFloatingTextFor(damage);
            CurrentHealth -= damage;
        }

        void CreateFloatingTextFor(int damage)
        {
            var NewDamageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            NewDamageText.GetComponent<TextMeshPro>().SetText($"{damage}");
            NewDamageText.GetComponent<TextMeshPro>().GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }
}
