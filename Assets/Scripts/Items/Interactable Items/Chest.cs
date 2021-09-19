using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Chest : MonoBehaviour, IItem, IInteractable
    {
        public Inventory Inventory;

        public string Name { get; set;}

        public string Description { get; set;}

        public int Value { get; set;}

        public int Weight { get; set;}

        public ItemTypes ItemType { get; set;}

        public GameObject GameObject => gameObject;

        public AudioSource Source { get; set; }

        public AudioClip InteractSound { get; set; }
        public Texture2D Icon { get; set; }

        public void Start()
        {
            tag = "Chest";
            Name = "Chest";
            Description = "A chest";
            Value = 0;
            Weight = 10;
            ItemType = ItemTypes.Chest;

            InteractSound = Resources.Load<AudioClip>("Sounds/Craft/Wood Creak 3");
            Source = gameObject.AddComponent<AudioSource>();
            Inventory = GetComponent<Inventory>();
        }

        public void Interact(){
            var itemBeingRetrieved = Inventory.RetrieveItemAt(0);
            if (itemBeingRetrieved == null)
                return;

            itemBeingRetrieved.transform.position = transform.position;
            itemBeingRetrieved.GetComponent<Rigidbody>().AddForce(transform.forward * itemBeingRetrieved.GetComponent<Rigidbody>().mass * 10);

            Source.PlayOneShot(InteractSound);
        }
    }
}
