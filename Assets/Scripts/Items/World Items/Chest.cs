using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Chest : WorldItem
    {
        public Inventory Inventory;
        public override void Start()
        {
            base.Start();
            tag = "Chest";
            Inventory = GetComponent<Inventory>();
        }

        public override void Interact(){
            base.Interact();

            var itemBeingRetrieved = Inventory.Items.FirstOrDefault();
            if (itemBeingRetrieved == null)
                return;

            Inventory.Items.RemoveAt(0);
            var clone = Instantiate(itemBeingRetrieved, transform.position, Quaternion.identity);
            GameObject.Destroy(itemBeingRetrieved);

            clone.GetComponent<Animator>().enabled = false;
            clone.GetComponent<Rigidbody>().isKinematic = false;
            clone.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
        }
    }
}
