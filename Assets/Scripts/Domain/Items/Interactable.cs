using UnityEngine;

public class Interactable : BaseItem {
    public AudioSource Source;
    public AudioClip InteractSound;
    public virtual void Interact() {
        Source.PlayOneShot(InteractSound);
    }
}