using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] float speed = 10;
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
        Destroy(gameObject);
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            OnDespawn();
        }
    }
}
