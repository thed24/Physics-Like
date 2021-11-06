using Assets.Scripts.Entities;
using Assets.Scripts.HoldableItems;
using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : Entity
    {
        public GameObject DamageText;
        void Update()
        {
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
        public override void TakeDamage(int damage)
        {
            CreateFloatingTextFor(damage);
            base.TakeDamage(damage);
        }

        private void CreateFloatingTextFor(int damage)
        {
            var NewDamageText = Instantiate(DamageText, transform.position, Quaternion.identity);
            NewDamageText.GetComponent<TextMeshPro>().SetText($"{damage}");
            NewDamageText.GetComponent<TextMeshPro>().GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }
    }
}
