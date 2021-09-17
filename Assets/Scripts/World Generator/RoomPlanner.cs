using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.World_Generator
{
    public class RoomPlanner
    {
        public Vector3 Scale;
        public Vector3 Position;

        public RoomPlanner SetPosition(Vector3 Position)
        {
            this.Position = Position;
            return this;
        }

        public RoomPlanner SetSize(Vector3 Scale)
        {
            this.Scale = Scale;
            return this;
        }

        public RoomPlanner AddPlayerSpawn()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(Position.x, Position.y + 5, Position.z);

            var chest = UnityExtensions.LoadPrefabFrom("Items/World/Chest");
            var starterWeapon = UnityExtensions.LoadPrefabFrom("Items/Holdable/Spear/Spear");
            var torch = UnityExtensions.LoadPrefabFrom("Items/Holdable/Torch/Torch");

            chest.transform.position = new Vector3(Position.x, Position.y + 1, Position.z);
            Debug.Log(chest.GetComponent<Chest>().Inventory);
            chest.GetComponent<Chest>().Inventory.Items.AddRange(new List<Item>(){
                starterWeapon.GetComponent<Item>(),
                torch.GetComponent<Item>()
            });

            return this;
        }

        public RoomPlanner AddEnemySpawnWithChanceOf(int chance)
        {
            int between0and100 = Random.Range(0, 100);
            if (between0and100 > chance)
            {
                var enemy = UnityExtensions.LoadPrefabFrom("NPCs/Enemy");
                enemy.transform.position = new Vector3(Position.x, Position.y + 5, Position.z);
            }
            return this;
        }
    }
}
