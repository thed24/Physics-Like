public interface ISkill
{
    string Name { get; }
    string Description { get; }
    int Cost { get; }
    float Cooldown { get; }
    float NextUse { get; set; }
    int Value { get; }
    void Reset();
    void Start();
    void OnUse();
    void OnHit(IEntity target);
}