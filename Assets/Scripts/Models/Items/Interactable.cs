using UnityEngine;

public class Interactable : BaseItem {
    public AudioSource Source { get; set; }
    public AudioClip InteractSound { get; set; }
    public virtual void Interact() {
        Source.PlayOneShot(InteractSound);
    }
}