using Assets.Scripts.Application.Items.HoldableItems;
using UnityEngine;

namespace Assets.Scripts.HoldableItems
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour, IHoldable, IInteractable
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int Damage { get; set; }
        [field: SerializeField] public float Cooldown { get; set; }
        [field: SerializeField] public float NextUse { get; set; }
        [field: SerializeField] public float Level { get; set; }
        [field: SerializeField] public WeaponType Type { get; set; }
        [field: SerializeField] public WeaponRarity Rarity { get; set; }

        [field: SerializeField] public Texture2D Icon { get; set; }
        [field: SerializeField] public AudioSource Source { get; set; }
        [field: SerializeField] public AudioClip EquipSound { get; set; }
        [field: SerializeField] public AudioClip DropSound { get; set; }
        [field: SerializeField] public AudioClip PickupSound { get; set; }
        [field: SerializeField] public AudioClip HitSound { get; set; }
        [field: SerializeField] public AudioClip SwingSound { get; set; }
        [field: SerializeField] public AnimationClip UseAnimation { get; set; }

        public GameObject GameObject { get; set; }

        public void Awake()
        {
            Source = GetComponent<AudioSource>();
            NextUse = 0;
            tag = "Weapon";
            GameObject = gameObject;
        }

        void OnCollisionEnter(Collision collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Source.PlayOneShot(HitSound);
                GetComponent<Animator>().SetTrigger("Hitstop");
            }
        }

        public void OnUse()
        {
            Source.PlayOneShot(SwingSound);

            GetComponent<Animator>().SetTrigger("Attack");
        }

        public void OnEquip(Transform parent)
        {
            Source.PlayOneShot(PickupSound);

            gameObject.layer = 6;
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Animator>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public void OnDrop()
        {
            Source.PlayOneShot(DropSound);

            gameObject.layer = 1;
            transform.position = transform.parent.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            transform.parent = null;

            gameObject.SetActive(true);
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<BoxCollider>().enabled = true;

            GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        }

        public void OnStore(Transform parent)
        {
            transform.parent = parent;
            gameObject.SetActive(false);
        }

        public void Interact()
        {
            gameObject.SetActive(false);
        }
    }
}
