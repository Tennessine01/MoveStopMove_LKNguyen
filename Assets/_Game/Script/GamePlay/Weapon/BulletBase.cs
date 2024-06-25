using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class BulletBase : GameUnit
{
    float speed = 8f;
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
        //if (owner != null)
        //{

        //    if (owner.isDespawn == true)
        //    {
        //        //Debug.Log("--------");
        //        OnDespawn();
        //    } 
        //}
    }
    public void OnInit( float damage)
    {
        startTime = 0f;
        this.damage = damage;
        LevelManager.Ins.OnDespawnLevel += OnDespawn;
    }

    public void OnDespawn()
    {
        owner = null;
        //Destroy(gameObject);
        SimplePool.Despawn(this);
        LevelManager.Ins.OnDespawnLevel -= OnDespawn;
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Constant.TAG_CHARACTER) || other.CompareTag(Constant.TAG_WALL))
        {
            if (owner != null)
            {
                if (other != owner)
                {
                    Character character = Cache.GetCharacter(other);
                    if (character == null) return;

                    if (character != null && character.isDespawn == false)
                    {
                        LevelManager.Ins.SetNameKillerAndVictim(owner.characterName, character.characterName);
                        character.OnHit(10f);
                        owner.AddScore(1);
                        OnDespawn();
                    }
                    else
                    {
                        OnDespawn();
                    }
                }
                else
                {
                    OnDespawn();
                } 
            }
            else { 
                OnDespawn(); 
            }    
        }
    }
    
}
