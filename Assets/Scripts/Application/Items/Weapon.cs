using UnityEngine;

namespace Assets.Scripts.HoldableItems
{
    public class Weapon : Item
    {
        public int Damage;

        public override void Start()
        {
            base.Start();
            tag = "Weapon";
        }

        void OnCollisionEnter(Collision collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                OnHit(enemy);
            }
        }

        public override void OnUse()
        {
            var weaponAnimator = GetComponent<Animator>();
            weaponAnimator.SetTrigger("Attack");
            Source.PlayOneShot(UseSound);
        }

        public override void OnPickup()
        {
            base.OnPickup();

            gameObject.layer = 6;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Animator>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public override void OnDrop()
        {
            base.OnDrop();

            gameObject.layer = 1;
            transform.position = transform.parent.position;
            transform.parent = null;
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        }

        private void OnHit(Entity target)
        {
            target.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }
}
