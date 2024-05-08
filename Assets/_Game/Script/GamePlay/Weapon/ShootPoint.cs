using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : WeaponBase
{
    [SerializeField] Transform bulletPoint;
    public override void Shoot(BulletType bulletType)
    {
        //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation* Quaternion.Euler(90f, 180f, 0f));
        BulletBase b = (BulletBase)SimplePool.Spawn<GameUnit>((PoolType)bulletType, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 180f, 0f));
        b.OnInit(10);
    }
}
