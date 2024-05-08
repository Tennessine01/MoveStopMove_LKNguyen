using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class BulletBase : GameUnit
{
    [SerializeField] float speed = 5;
    private float startTime = 0f;
    private float endTime = 2f; //time to delay
    float damage;
    public void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
        startTime += Time.deltaTime; // update time each frame

        if (startTime >= endTime) // check time 
        {
            OnDespawn();
        }
    }
    public void OnInit( float damage)
    {
        startTime = 0f;
        this.damage = damage;
    }

    public void OnDespawn()
    {
        //Destroy(gameObject);
        SimplePool.Despawn(this);
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Wall") || other.CompareTag("Player"))
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
