using System.Collections.Generic;
using Assets.Scripts.Builders;
using Assets.Scripts.Entities.Builders;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.World_Generator
{
    public class RoomPlanner
    {
        public Vector2 Scale;
        public Vector3 Position;

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
            player.transform.position = new Vector3(Position.x, Position.y + 6, Position.z);

            var chest = UnityExtensions.LoadPrefabFrom("Items/Interactable/Chest");

            chest.transform.position = new Vector3(Position.x, (float)(Position.y + 0.5), Position.z);
            chest.transform.Rotate(new Vector3(-90, 0, 0));

            chest.GetComponent<Chest>().Inventory.AddItems(new List<Item>() 
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

        public RoomPlanner AddEnemy()
        {
            var enemy = EnemyBuilder.BuildRandomEntity();
            enemy.transform.position = new Vector3(Position.x, Position.y + 5, Position.z);
            
            return this;
        }
    }
}
