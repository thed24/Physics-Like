using Assets.Scripts.Application.Items.HoldableItems;
using Assets.Scripts.HoldableItems;
using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Builders
{
    public class WeaponBuilder
    {
        public GameObject Weapon { get; set; }

        public WeaponBuilder NewWeapon(WeaponType type, WeaponRarity rarity, int level)
        {
            Weapon = new GameObject();
            Weapon.SetActive(false);
            Weapon.tag = "Weapon";

            var weapon = Weapon.AddComponent<Weapon>();
            weapon.Type = type;
            weapon.Rarity = rarity;
            weapon.Level = level;
            weapon.Damage = Random.Range(1, level);
            weapon.Cooldown = Random.Range(1, 2);
            weapon.Name = "Level " + level.ToString() + " " + rarity.ToString() + " " + type.ToString();

            return this;
        }

        public WeaponBuilder ExistingWeapon(GameObject existingWeapon, WeaponType type, WeaponRarity rarity, int level)
        {
            Weapon = existingWeapon;
            Weapon.SetActive(false);
            Weapon.tag = "Weapon";

            var weapon = Weapon.AddComponent<Weapon>();
            weapon.Type = type;
            weapon.Rarity = rarity;
            weapon.Level = level;
            weapon.Damage = Random.Range(1, level);
            weapon.Cooldown = Random.Range(1, 2);
            weapon.Name = "Level " + level.ToString() + " " + rarity.ToString() + " " + type.ToString();

            return this;
        }

        public WeaponBuilder WithAnimations(RuntimeAnimatorController animator, AnimationClip attackAnimation, AnimationClip idleAnimation)
        {
            var weapon = Weapon.GetComponent<Weapon>();

            var controller = animator as AnimatorController;
            controller.layers[0].stateMachine.states[1].state.motion = attackAnimation;
            controller.layers[0].stateMachine.states[0].state.motion = idleAnimation;

            weapon.GetComponent<Animator>().runtimeAnimatorController = controller;
            weapon.GetComponent<Animator>().enabled = false;

            return this;
        }

        public WeaponBuilder WithSounds(AudioClip pickupSound, AudioClip dropSound, AudioClip equipSound, AudioClip hitSound, AudioClip swingSound)
        {
            var weapon = Weapon.GetComponent<Weapon>();

            weapon.PickupSound = pickupSound;
            weapon.DropSound = dropSound;
            weapon.EquipSound = equipSound;
            weapon.HitSound = hitSound;
            weapon.SwingSound = swingSound;

            return this;
        }

        public WeaponBuilder WithIcon(Texture2D icon)
        {
            var weapon = Weapon.GetComponent<Weapon>();

            weapon.Icon = icon;

            return this;
        }

        public Weapon Build()
        {
            Weapon.SetActive(true);
            return Weapon.GetComponent<Weapon>();
        }

        public static Weapon BuildRandomWeaponAt()
        {
            var weaponTypes = Enum.GetNames(typeof(WeaponType));
            var weaponType = weaponTypes[Random.Range(0, weaponTypes.Count() - 1)];

            var weaponRarities = Enum.GetNames(typeof(WeaponRarity));
            var weaponRarity = weaponRarities[Random.Range(0, weaponRarities.Count() - 1)];

            var weaponFolder = $"Items/Holdable/{weaponType}/{weaponRarity}";

            var prefab = UnityExtensions.LoadPrefab($"{weaponFolder}/Weapon");
            var icon = Resources.Load<Texture2D>($"{weaponFolder}/Icon");

            var dropSound = Resources.Load<AudioClip>($"{weaponFolder}/Drop");
            var pickupSound = Resources.Load<AudioClip>($"{weaponFolder}/Pickup");
            var swingSound = Resources.Load<AudioClip>($"{weaponFolder}/Swing");
            var hitSound = Resources.Load<AudioClip>($"{weaponFolder}/Hit");
            var equipSound = Resources.Load<AudioClip>($"{weaponFolder}/Equip");

            var attackAnimation = Resources.Load<AnimationClip>($"{weaponFolder}/Attack");
            var idleAnimation = Resources.Load<AnimationClip>($"{weaponFolder}/Idle");
            
            var animator = Resources.Load<RuntimeAnimatorController>("Items/Holdable/Weapon Animator");

            var level = Random.Range(1, 5);

            return new WeaponBuilder()
                .ExistingWeapon(prefab, Enum.Parse<WeaponType>(weaponType), Enum.Parse<WeaponRarity>(weaponRarity), level)
                .WithAnimations(animator, attackAnimation, idleAnimation)
                .WithSounds(pickupSound, dropSound, equipSound, hitSound, swingSound)
                .WithIcon(icon)
                .Build();
        }
    }
}