using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : GameUnit
{
    [SerializeField] float speed = 8;
    float damage;
    public void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }
    public void OnInit( float damage)
    {

        this.damage = damage;
    }

    public void OnDespawn()
    {
        //Destroy(gameObject);
        SimplePool.Despawn(this);
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            OnDespawn();
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        OnDespawn();
    //    }
    //}
}
