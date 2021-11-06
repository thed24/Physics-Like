using Assets.Scripts.Items;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour {
    public int MaxHealth;
    public int MaxMana;
    public Inventory Equipped;
    public Inventory Inventory;
    public GameObject[] SkillPrefabs;
    public string Name;
    public int Level;
    public int Experience;
    public int Health;
    public int Mana;
    public int Dexterity;
    public int Strength;
    public int Intelligence;
    public int Luck;

    public virtual void Start(){
        MaxHealth = Health;
        MaxMana = Mana;
    }

    public virtual void TakeDamage(int damage) {
        Health -= damage;
    }

    public bool IsHoldingItemInHand(int handIndex){
        return Equipped.SeeItemAtSlot(handIndex);
    }

    public virtual void EquipItem(Item item, int handIndex){
        item.OnPickup();
        Equipped.AddItemAtSlot(handIndex, item);
    }

    public virtual void DropItem(int handIndex){
        var item = Equipped.GetItemAtSlot(handIndex);
        if (item != null) {
            item.OnDrop();
            item.gameObject.transform.parent = null;
        }
    }

    public virtual void TryUseHeldItem(int equippedItemIndex) {
        var item = Equipped.SeeItemAtSlot(equippedItemIndex);

        if (item != null && Time.time > item.Details.NextUse){
            item.OnUse();
            item.Details.NextUse = Time.time + item.Details.Cooldown;
        }
    }

    public virtual void TryUseSkill(int skillIndex) {
        var skillPrefab = SkillPrefabs?[skillIndex];
        if (skillPrefab == null) return;
        
        var skill = skillPrefab.GetComponent<Skill>();
        if (Time.time > skill.Details.NextUse && Mana >= skill.Details.Cost){
            var skillInstance = Instantiate(skillPrefab);
            skillInstance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
            skillInstance.GetComponent<Skill>().OnUse();
            skill.Details.NextUse = Time.time + skill.Details.Cooldown;
            Mana -= skill.Details.Cost;
        }
    }
}