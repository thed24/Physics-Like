using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Builders
{
    public class EnemyBuilder
    {
        public static Enemy BuildRandomEntity(Vector3 position = default(Vector3))
        {
            var enemy = Resources.Load<Object>("NPCs/Enemy");
            var enemyAsObject = Object.Instantiate(enemy, position, Quaternion.identity) as GameObject;

            var name = "Monster";
            var health = 10;
            var level = 1;

            return BuildEnemy(enemyAsObject, name, health, level);
        }

        public static Enemy BuildEnemy(GameObject enemy, string name, int health, int level)
        {
            enemy.AddComponent<Enemy>();
            enemy.GetComponent<Enemy>().Name = name;
            enemy.GetComponent<Enemy>().MaxHealth = health;
            enemy.GetComponent<Enemy>().Level = level;
            enemy.GetComponent<Enemy>().DamageText = Resources.Load<GameObject>("GUI/Floating Damage");

            return enemy.GetComponent<Enemy>();
        }
    }
}