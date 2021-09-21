using UnityEngine;

public interface IInteractable {
    AudioSource Source { get; set; }
    AudioClip InteractSound { get; set; }
    void Interact();
}