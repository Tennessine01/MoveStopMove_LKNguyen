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
    }
    public void OnInit( float damage)
    {
        startTime = 0f;
        this.damage = damage;
        LevelManager.Ins.OnDespawnLevel += OnDespawn;
    }
    public void SetOwnerForBullet(Character character)
    {
        owner = character;
    }

    public void OnDespawn()
    {
        owner = null;
        //Destroy(gameObject);
        LevelManager.Ins.OnDespawnLevel -= OnDespawn;
        SimplePool.Despawn(this);
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Constant.TAG_CHARACTER) || other.CompareTag(Constant.TAG_WALL))
        {
            Character character = Cache.GetCharacter(other);

            if (owner == null) return;
            if (owner != null && character != owner) 
            {
                if (character != owner)
                {
                    if (character == null)
                    {
                        return;
                    }
                    if (character != null && character.isDespawn == false)
                    {
                        LevelManager.Ins.SetNameKillerAndVictim(owner.characterName, character.characterName);
                        character.OnHit(10f);
                        owner.AddScore(1);
                        OnDespawn();
                    }
                    else{
                        OnDespawn();
                    }
                }
                else{
                    OnDespawn();
                } 
            }
            //else{ 
            //    OnDespawn(); 
            //}    
        }
    }
    
}
