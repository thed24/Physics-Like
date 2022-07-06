using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour, IEntity
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        [field: SerializeField] public int Health { get; set; }
        [field: SerializeField] public int MaxMana { get; set; }
        [field: SerializeField] public int Mana { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public int Experience { get; set; }
        [field: SerializeField] public int Dexterity { get; set; }
        [field: SerializeField] public int Strength { get; set; }
        [field: SerializeField] public int Intelligence { get; set; }
        [field: SerializeField] public int Luck { get; set; }
        [field: SerializeField] public string Name { get; set; }

        public GameObject DamageText;

        void Update()
        {
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            CreateFloatingTextFor(damage);
        }

        private void CreateFloatingTextFor(int damage)
        {
            var NewDamageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            NewDamageText.GetComponent<TextMeshPro>().SetText($"{damage}");
            NewDamageText.GetComponent<TextMeshPro>().GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }
}
