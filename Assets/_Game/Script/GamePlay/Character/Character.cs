using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Character : GameUnit
{
    [Header("Character Attributes")]

    ////Range
    [SerializeField] public float Range;
    //Movespeed
    [SerializeField] public float moveSpeed;
    ////AttackSpeed
    //[SerializeField] public float speed;
    ////Size
    [SerializeField] public float size;
    //Skin

    //pant
    [SerializeField] public int pantID;
    //vu khi tren tay
    [SerializeField] public int weaponID;
    [SerializeField] public int hatID;
    public GameObject hatPrefab;
    public GameObject weaponPrefab;
    [Space(10)] // Adds 10 pixels of space in the Inspector


    [Header("Other Attributes")]
    //Anim
    [SerializeField] public Animator animm;
    private string currentAnim = null;

    //Rigidbody
    [SerializeField] public Rigidbody rb;
    public CharacterAttackRange attackRange;
    public ShootPoint shootPoint;
    [HideInInspector]public WeaponBase weapon; //dung de chua prefab loai vu khi dang cam tren tay

    //Material
    public Renderer pant;
    //public Material panMaterial;

    //xac dinh xem character co phai player khong
    [HideInInspector] public bool isPlayer;

    //check death
    private float hp;
    public bool IsDead => hp <= 0;
    public bool isDespawn;

    //object chua vu khi
    public GameObject slotWeaponInHand;
    //vi tri de vu khi tren tay
    [SerializeField] public Transform weaponPosition;

    //vi tri de mu
    [SerializeField] public Transform hatPosition;
    public virtual void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        slotWeaponInHand.SetActive(true);
        hp = 20;
        attackRange.range = Range;
        TF.localScale = new Vector3(1, 1, 1)*size;
        //sinh vu khi
        //InstantiateWeapon();
        SetOwnerForBullet();
        attackRange.transform.localScale = new Vector3(1, 1, 1) * attackRange.range;


        //
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
            TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;
        }
    }
    //------------------------------------------------------------------
    public void InstantiateItem(int id, ShopType type)
    {
        switch (type)
        {
            case ShopType.Hat:
                InstantiateHat(id);
                break;
            case ShopType.Weapon:
                InstantiateWeapon(id);
                break;
            case ShopType.Pant:
                InstantiatePant(id);
                break;
            default:
                break;
        }
    }
    //-----------
    public void DestroyItem(int id, ShopType type)
    {
        switch (type)
        {
            case ShopType.Hat:
                DestroyHat();
                break;
            case ShopType.Pant:
                DestroyHat();
                break;
            case ShopType.Weapon:
                DestroyWeapon();
                break;
            default:
                break;
        }
    }
    public void InstantiateWeapon(int weaponID)
    {
        weaponPrefab = EquipemtManager.Ins.InstantiatePrefabById(weaponID, weaponPosition, ShopType.Weapon);
        if (weaponPrefab != null)
        {
            this.weapon = weaponPrefab.GetComponent<WeaponBase>();
        }
    }
    public void InstantiateHat(int hatID)
    {
        if (hatID == 0)
        {
            return;
        }
        hatPrefab = EquipemtManager.Ins.InstantiatePrefabById(hatID, hatPosition, ShopType.Hat);
    }
    public void InstantiatePant(int pantID)
    {
        if (pantID == 0)
        {
            return ;
        }
        pant.material = EquipemtManager.Ins.GetMaterialByID(pantID, ShopType.Pant);
        
    }
    public void DestroyHat()
    {
        Destroy(hatPrefab);
    }
    public void DestroyPant()
    {
        pant.material = EquipemtManager.Ins.defaultPantMaterial;
    }
    public void DestroyWeapon()
    {
        Destroy(weaponPrefab);
    }

    //------------------------------------------------
    public void CheckClosestEnemy()
    {
        attackRange.DetectNearCharacter();
        //Debug.Log(attackRange.targetCharacter != null);
        //Debug.Log(attackRange.characterList.Count);
    }
    public virtual void OnDead()
    {

    }
    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;

            if (IsDead)
            {
                hp = 0;
                OnDead();
            }
        }
    }
    public void ChangeAttackRagneByPercentage(float percent)
    {
        Range *= (percent/100 + 1);
    }
    public float ChangeSize(float percent)
    {
        return (percent / 100 + 1);
    }
    public void ChangeWeapon(int id)
    {
        weaponID = id;
    }

    public virtual void SetOwnerForBullet()
    {
        shootPoint.owner = this;
    }
}
