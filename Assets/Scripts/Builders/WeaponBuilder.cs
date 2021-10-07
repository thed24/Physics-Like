using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.HoldableItems;

namespace Assets.Scripts.Builders
{
    public class WeaponBuilder
    {
        private static List<string> WeaponTypes => new List<string> { "Axe", "Bow", "Mace", "Shield", "Spear", "Staff", "Wand" };

        public static Weapon BuildRandomWeaponAt(Vector3 position = default(Vector3))
        {
            var weaponType = WeaponTypes[Random.Range(0, WeaponTypes.Count - 1)];
            var weapon = Resources.LoadAll<Object>($"Items/Holdable/{weaponType}").FirstOrDefault(r => r.name.Split('_')[0].Contains($"{weaponType}"));
            var icon = Resources.LoadAll<Texture2D>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var whooshSound = Resources.LoadAll<AudioClip>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var attackSound = Resources.LoadAll<AudioClip>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var animation = Resources.LoadAll<AnimationClip>("Animations").Where(animation => !animation.name.Contains("Idle")).FirstOrDefault();
            var animator = Resources.LoadAll<RuntimeAnimatorController>("Animations").First(animator => animator.name.Contains(animation.name.Split(' ')[0]));
            var weaponGameObject = Object.Instantiate(weapon, position, Quaternion.identity) as GameObject;

            return BuildWeapon(
                weaponGameObject,
                weaponGameObject.name.Split('_')[0],
                Random.Range(1, 10),
                icon,
                animation,
                attackSound,
                whooshSound,
                whooshSound,
                animator
            );
        }

        public static Weapon BuildWeapon(GameObject weapon, string name, int damage, Texture2D icon, AnimationClip attackAnimation, AudioClip attackSound, AudioClip interactSound, AudioClip dropSound, RuntimeAnimatorController animator)
        {
            weapon.AddComponent<Weapon>();
            weapon.GetComponent<Weapon>().Details = new ItemDetails() { Icon = icon, Name = name };
            weapon.GetComponent<Weapon>().Damage = damage;
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