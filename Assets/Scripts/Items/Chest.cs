using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Chest : Interactable
    {
        public Inventory Inventory;

        public void Start()
        {
            tag = "Chest";
            Details = new ItemDetails() { Name = "Chest", Description = "A chest" };
            InteractSound = Resources.Load<AudioClip>("Sounds/Craft/Wood Creak 3");
            Source = gameObject.AddComponent<AudioSource>();
            Inventory = GetComponent<Inventory>();
        }

        public override void Interact(){
            base.Interact();

            var itemBeingRetrieved = Inventory.GetRandomItem();
            if (itemBeingRetrieved == null)
                return;

            itemBeingRetrieved.gameObject.SetActive(true);
            itemBeingRetrieved.transform.SetParent(null);
            itemBeingRetrieved.GetComponent<Rigidbody>().AddForce(transform.forward * 15);
        }
    }
}
