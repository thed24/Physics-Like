using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public string Name { get; set; }
        public Inventory Inventory;
        public AudioSource Source;
        public AudioClip InteractSound;

        public void Start()
        {
            tag = "Chest";
            Name = "Chest";
            InteractSound = Resources.Load<AudioClip>("Sounds/Craft/Wood Creak 3");
            Source = gameObject.AddComponent<AudioSource>();
            Inventory = GetComponent<Inventory>();
        }

        public void Interact()
        {
            Source.PlayOneShot(InteractSound);

            var itemBeingRetrieved = Inventory.GetRandomItem();
            if (itemBeingRetrieved.HasValue)
            {
                itemBeingRetrieved.Value.OnDrop();
            }
        }
    }
}
