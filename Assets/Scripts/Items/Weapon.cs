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

        public override void OnUse()
        {
            var weaponAnimator = GetComponent<Animator>();
            var animationInfo = weaponAnimator.GetCurrentAnimatorStateInfo(0);

            if (!animationInfo.IsName(UseAnimation.name))
            {
                weaponAnimator.Play(UseAnimation.name);
                Source.PlayOneShot(UseSound);
            }
            else
            {
                tag = animationInfo.IsName(UseAnimation.name) ? "ActiveWeapon" : "Weapon";
            }
        }

        public override void OnPickup()
        {
            base.OnPickup();

            GetComponent<Animator>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public override void OnDrop()
        {
            base.OnDrop();

            transform.position = transform.parent.position;
            transform.parent = null;
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        }
    }
}
