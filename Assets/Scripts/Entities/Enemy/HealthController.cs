using Assets.Scripts.Entities;
using Assets.Scripts.Items.Weapon;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Entity))]
    class HealthController : MonoBehaviour
    {
        public GameObject DamageText;

        private int Health;

        void Start()
        {
            var entity = gameObject.GetComponent<Entity>();
            Health = entity.MaxHealth;
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
            var weaponScript = gameObject.GetComponent<Weapon>();
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
