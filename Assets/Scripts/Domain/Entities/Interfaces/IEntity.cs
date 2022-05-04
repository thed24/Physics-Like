public interface IEntity
{
    int MaxHealth { get; }
    int MaxMana { get; }
    int Health { get; set; }
    int Mana { get; set; }
    int Dexterity { get; }
    int Strength { get; }
    int Intelligence { get; }
    int Luck { get; }
    int Level { get; }
    int Experience { get; }
    string Name { get; }

    void TakeDamage(int damage);
}