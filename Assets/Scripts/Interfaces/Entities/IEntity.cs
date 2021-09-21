using Assets.Scripts.Items;

public interface IEntity {
    Inventory Inventory { get; set; }
    string Name { get; set; }
    int Level { get; set; }
    int Experience { get; set; }
    int MaxHealth { get; set; }
    int MaxMana { get; set; }
    int Dexterity { get; set; }
    int Strength { get; set; }
    int Intelligence { get; set; }
    int Luck { get; set; }
}