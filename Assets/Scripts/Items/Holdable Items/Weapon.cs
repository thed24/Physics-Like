using UnityEngine;

namespace Assets.Scripts.HoldableItems
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour, IItem, IHoldable
    {
        public int Damage;
        public string Name { get; set; }

        public string Description { get; set; }

        public int Value { get; set; }

        public int Weight { get; set; }

        public ItemTypes ItemType { get; set; }

        public GameObject GameObject => gameObject;
        public AudioSource Source { get; set; }
        public AudioClip PickupSound { get; set; }
        public AudioClip DropSound { get; set; }
        public AudioClip UseSound { get; set; }
        public Animator Animator { get; set; }
        public AnimationClip UseAnimation { get; set; }
        public Texture2D Icon { get; set; }

        public void Start()
        {
            tag = "Weapon";
            Source = GetComponent<AudioSource>();
        }

        public void OnUse()
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

        public void OnPickup()
        {
            Source.PlayOneShot(PickupSound);
            GetComponent<Animator>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public void OnDrop()
        {
            Source.PlayOneShot(DropSound);
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
        }
    }
}
