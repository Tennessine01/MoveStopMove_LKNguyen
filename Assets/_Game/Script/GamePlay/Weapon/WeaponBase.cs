using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] float frameRate = 1.0f;


    //loai bullet
    [SerializeField] public BulletType bulletType;


    float time = 0.0f;
    //void Update()
    //{
    //    //time += Time.deltaTime;
    //    //if (time >= frameRate)
    //    //{
    //    //    time -= frameRate;
    //    //    Shoot();
    //    //}
    //    //Shoot();
    //}
    public virtual void Shoot(BulletType bulletType, float size)
    {

    }
}
