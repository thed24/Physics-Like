using System;
using System.Collections.Generic;
using Assets.Scripts.Builders;
using Assets.Scripts.Entities.Builders;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.World_Generator
{
    public class RoomPlanner
    {
        public Transform World;
        public Vector2 Scale;
        public Vector3 Position;

        public RoomPlanner(Transform world)
        {
            World = world;
        }

        public RoomPlanner SetPosition(Vector3 Position)
        {
            this.Position = Position;
            return this;
        }

        public RoomPlanner SetSize(Vector2 Scale)
        {
            this.Scale = Scale;
            return this;
        }

        public RoomPlanner AddPlayerSpawn()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(Position.x + 2, Position.y + 2, Position.z);

            var chest = UnityExtensions.LoadPrefabFrom("Items/Interactable/Chest/Chest");
            chest.transform.position = new Vector3(Position.x, (float)(Position.y + 0.5), Position.z);
            chest.transform.Rotate(new Vector3(-90, 0, 0));
            chest.GetComponent<Chest>().Inventory.AddItems(new List<IHoldable>()
            {
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt(),
                WeaponBuilder.BuildRandomWeaponAt()
            });

            return this;
        }

        public RoomPlanner AddEnemy(float chance)
        {
            if (UnityEngine.Random.value < chance)
            {
                var enemy = EnemyBuilder.BuildRandomEntity();
                enemy.transform.position = new Vector3(Position.x, Position.y + 5, Position.z);
            }

            return this;
        }

        public RoomPlanner AddTorches()
        {
            foreach (var direction in Enum.GetValues(typeof(Direction)) as Direction[])
            {
                var torch = UnityExtensions.LoadPrefabFrom("Items/Torch");
                torch.transform.position = new Vector3
                (
                    Position.x + UnityEngine.Random.Range(-Scale.x / 2, Scale.x / 2),
                    Position.y + Scale.y / 2,
                    Position.z + UnityEngine.Random.Range(0, Scale.x - 1)
                );
            }
            return this;
        }
    }
}
