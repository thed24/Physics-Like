using UnityEngine;

public interface IHoldable
{
    string Name { get; }
    float Cooldown { get; }
    float NextUse { get; set; }
    Texture2D Icon { get; }
    GameObject GameObject { get; set; }
    void OnEquip(Transform parent);
    void OnDrop();
    void OnUse();
    void OnStore(Transform parent);
}