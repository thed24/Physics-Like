using UnityEngine;

public interface IHoldable {
    AudioSource Source { get; set; }
    Animator Animator { get; set; }
    AnimationClip UseAnimation { get; set; }
    AudioClip PickupSound { get; set; }
    AudioClip DropSound { get; set; }
    AudioClip UseSound { get; set; }
    void OnPickup();
    void OnDrop();
    void OnUse();
}