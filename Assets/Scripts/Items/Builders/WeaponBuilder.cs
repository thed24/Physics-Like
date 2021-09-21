using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.HoldableItems;

namespace Assets.Scripts.Builders
{
    public class WeaponBuilder
    {
        private static List<Object> weapons => Resources.LoadAll("Items/Holdable/Weapons").ToList();
        private static List<AudioClip> whooshSounds => Resources.LoadAll<AudioClip>("Sounds/Whoosh").ToList();
        private static List<AudioClip> attackSounds => Resources.LoadAll<AudioClip>("Sounds/Combat").ToList();
        private static List<AnimationClip> animations => Resources.LoadAll<AnimationClip>("Animations").ToList();
        private static List<RuntimeAnimatorController> animators => Resources.LoadAll<RuntimeAnimatorController>("Animations").ToList();

        public static Weapon BuildRandomWeaponAt(Vector3 position = default(Vector3))
        {
            var weapon = weapons[Random.Range(0, weapons.Count)];
            var weaponGameObject = Object.Instantiate(weapon, position, Quaternion.identity) as GameObject;

            var attackAnimation = animations.Where(animation => !animation.name.Contains("Idle")).ToList()[Random.Range(0, animations.Where(animation => !animation.name.Contains("Idle")).ToList().Count)];
            var attackSound = attackSounds[Random.Range(0, attackSounds.Count)];
            var whooshSound = whooshSounds[Random.Range(0, whooshSounds.Count)];
            var animator = animators.First(animator => animator.name.Contains(attackAnimation.name.Split(' ')[0]));

            return BuildWeapon(
                weaponGameObject,
                weaponGameObject.name.Split('_')[0],
                Random.Range(1, 10),
                new Texture2D(1, 1),
                attackAnimation,
                attackSound,
                whooshSound,
                whooshSound,
                animator
            );
        }

        public static Weapon BuildWeapon(GameObject weapon, string name, int damage, Texture2D icon, AnimationClip attackAnimation, AudioClip attackSound, AudioClip interactSound, AudioClip dropSound, RuntimeAnimatorController animator)
        {
            weapon.AddComponent<Weapon>();
            weapon.GetComponent<Weapon>().Name = name;
            weapon.GetComponent<Weapon>().Damage = damage;
            weapon.GetComponent<Weapon>().Icon = icon;
            weapon.GetComponent<Weapon>().UseAnimation = attackAnimation;
            weapon.GetComponent<Weapon>().UseSound = attackSound;
            weapon.GetComponent<Weapon>().PickupSound = attackSound;
            weapon.GetComponent<Weapon>().DropSound = attackSound;
            weapon.GetComponent<Animator>().runtimeAnimatorController = animator;
            weapon.GetComponent<Animator>().enabled = false;

            return weapon.GetComponent<Weapon>();
        }
    }
}