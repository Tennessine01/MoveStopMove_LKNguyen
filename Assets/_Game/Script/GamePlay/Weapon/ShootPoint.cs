using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : WeaponBase
{
    [SerializeField] Transform bulletPoint;
    [SerializeField] Transform parentRotation;
    [SerializeField] BulletBase bulletBasePrefab;

    public override void Shoot()
    {
        base.Shoot();

        BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation* Quaternion.Euler(90f, 180f, 0f));

        b.OnInit(10);
    }
}
