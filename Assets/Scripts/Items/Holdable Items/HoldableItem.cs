using UnityEngine;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(Animator))]
    public class HoldableItem : Item
    {
        public override void Start()
        {
            base.Start();
            tag = "InteractableItem";
        }

        public override void Interact(){
            base.Interact();
            Destroy(gameObject);
        }
    }
}
