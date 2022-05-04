using Assets.Scripts;
using UnityEngine;

public class FireBall : MonoBehaviour, ISkill
{
    public GameObject Explosion;

    [field: SerializeField] public string Name { get; }
    [field: SerializeField] public string Description { get; }
    [field: SerializeField] public int Cost { get; }
    [field: SerializeField] public float Cooldown { get; }
    [field: SerializeField] public float NextUse { get; set; }
    [field: SerializeField] public int Value { get; }

    public AudioSource Source;
    public AudioClip UseSound;
    public AudioClip HitSound;

    public void Start()
    {
        Source = GetComponent<AudioSource>();
        NextUse = 0f;
    }

    public void Reset()
    {
        NextUse = 0;
    }

    public void OnUse()
    {
        Source.PlayOneShot(UseSound);
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1000);
    }

    public void OnHit(IEntity target)
    {
        Source.PlayOneShot(HitSound);

        var damage = Value;
        target.TakeDamage(damage);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<IEntity>();
        if (enemy != null)
        {
            OnHit(enemy);
        }
        Destroy(gameObject);
        Instantiate(Explosion, transform.position, Quaternion.identity);
    }
}