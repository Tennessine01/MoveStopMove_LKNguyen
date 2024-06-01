using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : WeaponBase
{
    public Character owner;
    [SerializeField] Transform bulletPoint;
    public BulletBase b;
    private List<BulletBase> listbullet = new();
    public override void Shoot(BulletType bulletType)
    {
        //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation* Quaternion.Euler(90f, 180f, 0f));
        b = (BulletBase)SimplePool.Spawn<GameUnit>((PoolType)bulletType, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 180f, 0f));
        b.OnInit(10);
        b.owner = owner;
        listbullet.Add(b);
    }
    public void DespawnBullet()
    {
        for (int i = 0; i < listbullet.Count; i++)
        {
            SimplePool.Despawn(listbullet[i]);
        }
    }
}
