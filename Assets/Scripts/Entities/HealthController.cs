using Assets.Scripts.Items;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    class HealthController : MonoBehaviour
    {
        public int MaxHealth;
        public GameObject DamageText;

        private int Health;

        void Start()
        {
            Health = MaxHealth;
        }

        void Update()
        {
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("ActiveWeapon"))
            {
                TakeDamageFrom(collision.gameObject);
            }
        }

        private void TakeDamageFrom(GameObject gameObject)
        {
            var weaponScript = gameObject.GetComponent<WeaponStats>();
            var damage = weaponScript.Damage;

            CreateFloatingTextFor(damage);
            Health -= damage;
        }

        private void CreateFloatingTextFor(int damage)
        {
            var NewDamageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            NewDamageText.GetComponent<TextMeshPro>().SetText($"{damage}");
            NewDamageText.GetComponent<TextMeshPro>().GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }
}
