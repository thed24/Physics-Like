using Assets.Scripts.Entities;
using Assets.Scripts.HoldableItems;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Entity))]
    class HealthController : MonoBehaviour
    {
        public GameObject DamageText;

        private int CurrentHealth;

        void Start()
        {
            var entity = gameObject.GetComponent<Entity>();
            CurrentHealth = entity.MaxHealth;
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
                TakeDamageFrom(collision.gameObject);
            }
        }

        private void TakeDamageFrom(GameObject gameObject)
        {
            var weaponScript = gameObject.GetComponent<Weapon>();
            var damage = weaponScript.Damage;

            CreateFloatingTextFor(damage);
            CurrentHealth -= damage;
        }

        private void CreateFloatingTextFor(int damage)
        {
            var NewDamageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            NewDamageText.GetComponent<TextMeshPro>().SetText($"{damage}");
            NewDamageText.GetComponent<TextMeshPro>().GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }
}
