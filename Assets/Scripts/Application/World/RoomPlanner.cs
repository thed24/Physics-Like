using Assets.Scripts.Builders;
using Assets.Scripts.Entities.Builders;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World_Generator
{
    public class RoomPlanner
    {
        public Transform World { get; }
        public Transform Enemies { get; }
        public Transform Items { get; }

        public Vector2 Scale;
        public Vector3 Position;

        public RoomPlanner(Transform world, Transform enemies, Transform items)
        {
            World = world;
            Enemies = enemies;
            Items = items;
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

            var randomWeaponAmount = Random.Range(1, 10);
            var weapons = new List<IHoldable>();
            
            for (int i = 0; i < randomWeaponAmount; i++)
            {
                weapons.Add(WeaponBuilder.BuildRandomWeaponAt());
            }

            var chest = UnityExtensions.LoadPrefab("Items/Interactable/Chest/Chest", Enemies);
            chest.transform.position = new Vector3(Position.x, 1f, Position.z);
            chest.GetComponent<Chest>().Inventory.AddItems(weapons);

            return this;
        }

        public RoomPlanner AddEnemy(float chance)
        {
            if (Random.value < chance)
            {
                var enemy = EnemyBuilder.BuildRandomEntity();
                enemy.transform.parent = Enemies;
                enemy.transform.position = new Vector3(Position.x, Position.y + 5, Position.z);
            }

            return this;
        }
        
        public RoomPlanner AddTorches(Vector3? location = null)
        {
            //var torch = UnityExtensions.LoadPrefab("Items/Interactable/Torch/Torch", Items);
            //torch.transform.position = location ?? new Vector3(Position.x, Scale.y / 2, Position.z);
            
            return this;
        }
    }
}
