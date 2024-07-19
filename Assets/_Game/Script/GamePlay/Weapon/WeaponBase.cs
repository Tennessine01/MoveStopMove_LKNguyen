using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] float frameRate = 1.0f;
    //loai bullet
    [SerializeField] public BulletType bulletType;
    
    public virtual void Shoot(BulletType bulletType, float size)
    {
        DeActivateWeapon();
    }
    public void ActivateWeapon()
    {
        gameObject.SetActive(true);
    }
    public void DeActivateWeapon()
    {
        gameObject.SetActive(false);
    }
}
