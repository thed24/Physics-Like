using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Item : BaseItem {
    public AudioSource Source { get; set; }
    public Animator Animator { get; set; }
    public AnimationClip UseAnimation { get; set; }
    public AudioClip PickupSound { get; set; }
    public AudioClip DropSound { get; set; }
    public AudioClip UseSound { get; set; }
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
    }
}