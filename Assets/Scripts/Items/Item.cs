using UnityEngine;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class Item : MonoBehaviour
    {
        [HideInInspector]
        public AudioSource Source;
        public string Name;
        public AudioClip InteractSound;
        public Texture2D Icon;
        public virtual void Start()
        {
            Source = gameObject.AddComponent<AudioSource>();
        }
        public virtual void Interact(){
            Source.PlayOneShot(InteractSound);
        }
    }
}
