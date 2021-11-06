using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Item : BaseItem {
    public AudioSource Source;
    public Animator Animator;
    public AnimationClip UseAnimation;
    public AudioClip PickupSound;
    public AudioClip DropSound;
    public AudioClip UseSound;
    public virtual void OnPickup(){
        Source.PlayOneShot(PickupSound);
    }
    public virtual void OnDrop(){
        Source.PlayOneShot(DropSound);
    }
    public virtual void OnUse(){
        Source.PlayOneShot(UseSound);
    }
    public virtual void Start(){
        Source = GetComponent<AudioSource>();
        Details.NextUse = 0;
    }
}