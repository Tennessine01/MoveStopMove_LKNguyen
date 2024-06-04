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
    [SerializeField] public int pantID = 0;
    //vu khi tren tay
    [SerializeField] public int weaponID = 0;
    [SerializeField] public int hatID = 0;
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
    [SerializeField] private float hp;
    public bool IsDead => hp <= 0;
    public bool isDespawn;

    //object chua vu khi
    public GameObject slotWeaponInHand;
    //vi tri de vu khi tren tay
    [SerializeField] public Transform weaponPosition;

    //vi tri de mu
    [SerializeField] public Transform hatPosition;
    //public virtual void Start()
    //{
    //    OnInit();
    //}

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
            if (attackRange.targetCharacter.isDespawn == true)
            {
                Debug.Log("gggggggggggggggg");
                attackRange.characterList.Remove(attackRange.targetCharacter);
                attackRange.targetCharacter = null;
                if (this is Bot bot)
                {
                    Debug.Log("fffffffff");
                    bot.ChangeState(new IdleState());
                }

            }
            else{
                //quay ve huong ke dich
                TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;
            }
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
        if(hatPrefab != null)
        {
            Destroy(hatPrefab);
        }
        hatID = 0;
    }
    public void DestroyPant()
    {
        pant.material = EquipemtManager.Ins.defaultPantMaterial;
    }
    public void DestroyWeapon()
    {
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab);
        }
        weaponID = 0;
    }
    public void ResetItem()
    {
        DestroyHat();
        DestroyPant();
        DestroyWeapon();
    }

    //------------------------------------------------
    public void CheckClosestEnemy()
    {
        attackRange.DetectNearCharacter();
    }
    public virtual void OnDead()
    {
        isDespawn = true;
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

    public virtual void OnDespawn()
    {
        ResetItem();
        shootPoint.DespawnBullet();
        ClearListEnemyInAttackRange();
    }
    public void IncreaseHP(int aa)
    {
        hp += aa;
    }
    //public void ChangeAttackRagneByPercentage(float percent)
    //{
    //    Range *= (percent/100 + 1);
    //}
    //public float ChangeSize(float percent)
    //{
    //    return (percent / 100 + 1);
    //}
    //public void ChangeWeapon(int id)
    //{
    //    weaponID = id;
    //}

    public virtual void SetOwnerForBullet()
    {
        shootPoint.owner = this;
    }
    public void ClearListEnemyInAttackRange()
    {
        if(attackRange.characterList.Count > 0)
        {
            attackRange.characterList.Clear();
        }
        if(attackRange.targetCharacter != null)
        {
            attackRange.targetCharacter = null;
        }
    }
}
