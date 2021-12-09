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
    public Transform[] Hands;
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

    public bool IsHoldingItemInHand(int hand){
        return Equipped.SeeItemAtSlot(hand);
    }

    public virtual void EquipItem(Item item, int hand){
        item.OnPickup();
        Equipped.AddItemAtSlot(hand, item);

        item.gameObject.SetActive(true);
        item.transform.SetParent(Hands[hand]);
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.transform.rotation = Quaternion.identity;
        item.GetComponent<Item>().OnPickup();
    }

    public virtual void DropItem(int hand){
        var item = Equipped.GetItemAtSlot(hand);
        if (item != null) {
            item.OnDrop();
            item.gameObject.transform.parent = null;
        }
    }

    public virtual void TryUseHeldItem(int hand) {
        var item = Equipped.SeeItemAtSlot(hand);

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