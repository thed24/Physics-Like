using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Skill : MonoBehaviour {
    public SkillDetails Details;
    public AudioSource Source;
    public AudioClip UseSound;
    public AudioClip HitSound;
    public virtual void Start(){
        Source = GetComponent<AudioSource>();
        Details.NextUse = 0f;
    }
    public virtual void OnUse(){
        Source.PlayOneShot(UseSound);
    }
    public virtual void OnHit(Entity target){
        Source.PlayOneShot(HitSound);
    }
}