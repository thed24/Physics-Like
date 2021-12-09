using Assets.Scripts;
using UnityEngine;

public class FireBall : Skill
{
    public GameObject Explosion;

    public override void OnUse(){
        base.OnUse();
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1000);
    }

    public override void OnHit(Entity target){
        base.OnHit(target);

        var damage = Details.Damage;
        target.TakeDamage(damage);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            OnHit(enemy);
        }
        Destroy(gameObject);
        Instantiate(Explosion, transform.position, Quaternion.identity);
    }
}