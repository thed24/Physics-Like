using UnityEngine;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(Animator))]
    public class WorldItem : Item
    {
        public override void Start()
        {
            base.Start();
            tag = "WorldItem";
        }
    }
}
