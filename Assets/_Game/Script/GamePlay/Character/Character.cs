using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Character : GameUnit
{
   
    //Anim
    [SerializeField] public Animator animm;
    private string currentAnim = null;
    //Rigidbody
    [SerializeField] public Rigidbody rb;
    ////Range
    //[SerializeField] public float range;
    //Movespeed
    [SerializeField] public float moveSpeed;
    ////AttackSpeed
    //[SerializeField] public float speed;
    ////Size
    //[SerializeField] public float size;
    //Skin

    

    public CharacterAttackRange attackRange;
    public ShootPoint shootPoint;
    //vu khi tren tay
    [SerializeField] string weaponID;
    [HideInInspector]public WeaponBase weapon; //dung de chua prefab loai vu khi dang cam tren tay

    //xac dinh xem character co phai player khong
    [HideInInspector] public bool isPlayer;

    //check death
    bool isDead = false;

    //object chua vu khi
    public GameObject slotWeaponInHand;
    //vi tri de vu khi tren tay
    [SerializeField] public Transform weaponPosition;
    public void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        slotWeaponInHand.SetActive(true);


        //sinh vu khi
        InstantiateWeapon();

    }
    // Update is called once per frame
    public virtual void Update()
    {


    }


    //-----------------------------------------------------------

    public void ChangeAnim(string animName)
    {
        if(currentAnim == null)
        {
            currentAnim = animName;
        }
        if (currentAnim != animName )
        {
            animm.ResetTrigger(currentAnim);
            animm.SetTrigger(animName);
            currentAnim = animName;
        }
    }
    public virtual void AttackWhenStop()
    {
        if (attackRange.targetCharacter != null)
        {
            //quay ve huong ke dich
            transform.forward = (attackRange.targetCharacter.transform.position - transform.position).normalized;

        }
    }
    public void InstantiateWeapon()
    {
        GameObject weapon = EquipemtManager.Ins.InstantiatePrefabById(weaponID, weaponPosition);
        if (weapon != null)
        {
            this.weapon = weapon.GetComponent<WeaponBase>();
        }
    }
    public void CheckClosestEnemy()
    {
        attackRange.DetectNearCharacter();
        //Debug.Log(attackRange.targetCharacter != null);
        //Debug.Log(attackRange.characterList.Count);
    }


}
