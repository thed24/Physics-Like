using Assets.Scripts.Items;
using UnityEngine;

public class Entity : MonoBehaviour {
    public Inventory Equipped { get; set; }
    public Inventory Inventory { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int MaxHealth { get; set; }
    public int MaxMana { get; set; }
    public int Dexterity { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Luck { get; set; }
}