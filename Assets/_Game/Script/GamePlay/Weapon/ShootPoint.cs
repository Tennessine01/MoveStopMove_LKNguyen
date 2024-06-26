using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : WeaponBase
{
    [SerializeField] private Character owner;
    [SerializeField] Transform bulletPoint;
    public BulletBase b;
    //private List<BulletBase> listbullet = new();
    public override void Shoot(BulletType bulletType, float size)
    {
        //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation* Quaternion.Euler(90f, 180f, 0f));
        b = (BulletBase)SimplePool.Spawn<GameUnit>((PoolType)bulletType, bulletPoint.position, bulletPoint.rotation * Quaternion.Euler(90f, 180f, 0f));
        b.OnInit(10);
        b.SetOwnerForBullet(owner);
        b.TF.localScale = size*Vector3.one*40;
        //listbullet.Add(b);
    }
    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }
    //public void DespawnBullet()
    //{
    //    for (int i = 0; i < listbullet.Count; i++)
    //    {
    //        SimplePool.Despawn(listbullet[i]);
    //    }
    //}
}
