using UnityEngine;

namespace Assets.Scripts.Items.Weapon
{
    public class Weapon : HoldableItem
    {
        public int Damage;
        public AnimationClip AttackAnimation;
        public AudioClip AttackSound;

        public override void Start()
        {
            base.Start();
            tag = "Weapon";
        }
    }
}
