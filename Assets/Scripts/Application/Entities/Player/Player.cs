using Assets.Scripts.Items;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HandController))]
public class Player : MonoBehaviour, IEntity, IUseSkills, IHoldItems
{
    [field: SerializeField] public int MaxHealth { get; set; }
    [field: SerializeField] public int MaxMana { get; set; }
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int Mana { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public int Experience { get; set; }
    [field: SerializeField] public int Dexterity { get; set; }
    [field: SerializeField] public int Strength { get; set; }
    [field: SerializeField] public int Intelligence { get; set; }
    [field: SerializeField] public int Luck { get; set; }

    public Inventory Equipped;
    public Inventory Inventory;
    public GameObject[] Skills;
    public Transform[] Hands;

    public void Start()
    {
        MaxHealth = Health;
        MaxMana = Mana;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public bool IsHoldingItemInHand(int hand)
    {
        return Equipped.SeeItemAtSlot(hand).HasValue;
    }

    public void EquipItem(IHoldable item, int hand)
    {
        item.OnEquip(Hands[hand].transform);
        Equipped.AddItemAtSlot(hand, item);
    }

    public void DropItem(int hand)
    {
        var item = Equipped.GetItemAtSlot(hand);

        if (item.HasValue)
        {
            item.Value.OnDrop();
        }
    }

    public void UseHeldItem(int hand)
    {
        var item = Equipped.SeeItemAtSlot(hand);

        if (item.HasValue && Time.time > item.Value.NextUse)
        {
            item.Value.OnUse();
            item.Value.NextUse = Time.time + item.Value.Cooldown;
        }
    }

    public void UseSkill(int skillIndex)
    {
        var skillPrefab = Skills[skillIndex];
        if (skillPrefab == null) return;

        var skill = skillPrefab.GetComponent<ISkill>();
        if (Time.time > skill.NextUse && Mana >= skill.Cost)
        {
            var skillInstance = Instantiate(skillPrefab);
            skillInstance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
            skillInstance.GetComponent<ISkill>().OnUse();
            skill.NextUse = Time.time + skill.Cooldown;
            Mana -= skill.Cost;
        }
    }
}