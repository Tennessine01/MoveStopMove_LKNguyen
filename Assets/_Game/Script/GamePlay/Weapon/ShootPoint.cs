using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : WeaponBase
{
    [SerializeField] Transform bulletPoint;
    public override void Shoot()
    {
        base.Shoot();

        //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation* Quaternion.Euler(90f, 180f, 0f));
        BulletBase b = SimplePool.Spawn<BulletBase>(PoolType.Bullet_1, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 180f, 0f));
        b.OnInit(10);
    }
}
