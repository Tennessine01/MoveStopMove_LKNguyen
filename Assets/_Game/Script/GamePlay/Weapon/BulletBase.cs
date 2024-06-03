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
    public Character owner;
    public void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
        startTime += Time.deltaTime; // update time each frame

        if (startTime >= endTime) // check time 
        {
            OnDespawn();
        }
        if (owner != null)
        {

            if (owner.isDespawn == true)
            {
                //Debug.Log("--------");
                OnDespawn();
            } 
        }
    }
    public void OnInit( float damage)
    {
        startTime = 0f;
        this.damage = damage;
    }

    public void OnDespawn()
    {
        owner = null;
        //Destroy(gameObject);
        SimplePool.Despawn(this);
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Wall") || other.CompareTag("Player"))
        {
            if (owner != null)
            {
                if (other != owner)
                {
                    Cache.GetCharacter(other).OnHit(10f);
                    OnDespawn();
                }
                else
                {
                    OnDespawn();
                } 
            }
            //if(owner is Player)
            //{
            //    if(Cache.GetCharacter(other).isDespawn == true)
            //    {

            //    }
            //}
            
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        OnDead();
    //    }
    //}
}
