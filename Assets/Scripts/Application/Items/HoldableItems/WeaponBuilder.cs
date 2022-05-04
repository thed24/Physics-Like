using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.HoldableItems;
using UnityEditor.Animations;

namespace Assets.Scripts.Builders
{
    public class WeaponBuilder
    {
        private static List<string> WeaponTypes => new List<string> { "Axe", /*"Bow", "Mace", /*"Shield", "Spear", /*"Staff", "Wand", "Great Axe", "Great Spear", "Great Mace"*/ };
        private static List<string> WeaponRarities => new List<string> { "Basic", "Medium", "Epic" };
        public static Weapon BuildRandomWeaponAt(Vector3 position = default(Vector3))
        {
            var weaponType = WeaponTypes[Random.Range(0, WeaponTypes.Count - 1)];
            var weaponRarity = WeaponRarities[Random.Range(0, WeaponRarities.Count - 1)];
            var weapon = Resources.LoadAll<Object>($"Items/Holdable/{weaponType}").FirstOrDefault(r => r.name.Contains(weaponRarity));
            var icon = Resources.LoadAll<Texture2D>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var whooshSound = Resources.LoadAll<AudioClip>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var attackSound = Resources.LoadAll<AudioClip>($"Items/Holdable/{weaponType}").FirstOrDefault();
            var attackAnimation = Resources.LoadAll<AnimationClip>($"Items/Holdable/{weaponType}").Where(animation => !animation.name.Contains("Idle")).FirstOrDefault();
            var idleAnimation = Resources.LoadAll<AnimationClip>($"Items/Holdable/{weaponType}").Where(animation => animation.name.Contains("Idle")).FirstOrDefault();
            var animator = Resources.Load<RuntimeAnimatorController>("Items/Holdable/Weapon Animator");
            var weaponGameObject = Object.Instantiate(weapon, position, Quaternion.identity) as GameObject;

            return BuildWeapon(
                weaponGameObject,
                $"{weaponType} ({weaponRarity})",
                Random.Range(1, 10),
                icon,
                attackAnimation,
                idleAnimation,
                attackSound,
                whooshSound,
                whooshSound,
                animator
            );
        }

        public static Weapon BuildWeapon(GameObject weapon, string name, int damage, Texture2D icon, AnimationClip attackAnimation, AnimationClip idleAnimation, AudioClip attackSound, AudioClip interactSound, AudioClip dropSound, RuntimeAnimatorController animator)
        {
            var controller = animator as AnimatorController;
            controller.layers[0].stateMachine.states[1].state.motion = attackAnimation;
            controller.layers[0].stateMachine.states[0].state.motion = idleAnimation;

            weapon.AddComponent<Weapon>();
            weapon.GetComponent<Weapon>().Icon = icon;
            weapon.GetComponent<Weapon>().Name = name;
            weapon.GetComponent<Weapon>().Cooldown = 0.25f;
            weapon.GetComponent<Weapon>().Damage = damage;
            weapon.GetComponent<Weapon>().UseAnimation = attackAnimation;
            weapon.GetComponent<Weapon>().UseSound = attackSound;
            weapon.GetComponent<Weapon>().PickupSound = dropSound;
            weapon.GetComponent<Weapon>().DropSound = dropSound;
            weapon.GetComponent<Weapon>().EquipSound = dropSound;
            weapon.GetComponent<Animator>().runtimeAnimatorController = controller;
            weapon.GetComponent<Animator>().enabled = false;

            return weapon.GetComponent<Weapon>();
        }
    }
}