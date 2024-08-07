using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Character : GameUnit
{
    [Header("Character Attributes")]

    ////Range
    public float Range = 6f;
    //Movespeed
    public float moveSpeed;
    ////AttackSpeed
    //[SerializeField] public float speed;
    ////Size
    public float size = 1;
    public int score;
    public int Score => score;
    //Skin
    public GameObject targetMark;

    //Name
    public string characterName;

    [SerializeField] Transform indicatorPosition;
    [SerializeField] protected TargetIndicator targetIndicator  = null;
    //pant
    public int pantID = 0;
    //vu khi tren tay
    public int weaponID = 0;
    public int shieldID = 0;
    public int hatID = 0;
    public GameObject hatPrefab;
    public GameObject weaponPrefab;
    [Space(10)] // Adds 10 pixels of space in the Inspector


    [Header("Other Attributes")]
    //Anim
    public Animator animm;
    private string currentAnim = null;

    //Rigidbody
    public Rigidbody rb;
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

    //object chua vu khi dung de bat tat trang thai khi dang nem
    public GameObject slotWeaponInHand;
    //vi tri de vu khi tren tay phai
    public Transform weaponPosition;
    //vi tri de khien ten tay trai
    public Transform shieldPosition;

    //vi tri de mu
    public Transform hatPosition;

    public List<Character> targetsList = new();
    public Character target;
    //vi tri cua muc tieu
    private Vector3 targetPoint;

    //
    public List<GameObject> listShields = new List<GameObject>();
    //public List<GameObject> listHats = new List<GameObject>();


    public virtual void OnInit()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        slotWeaponInHand.SetActive(true);
        target = null;
        hp = 10;
        size = 1;
        SetSize(size);
        score = 0;
        //attackRange.range = Range;
        TF.localScale = Vector3.one*size;
        //sinh vu khi
        //InstantiateWeapon();
        SetOwnerForBullet();
        attackRange.transform.localScale = Vector3.one * Range;
        targetsList.Clear();
        //tao target indicator
        //InstantiateTargetIndicator();
        //
    }
    public virtual void InstantiateTargetIndicator()
    {

        targetIndicator = SimplePool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        targetIndicator.SetTarget(indicatorPosition);
    }
    public virtual void Update()
    {


    }

    //them muc tieu
    public virtual void AddTarget(Character target)
    {
        targetsList.Add(target);
    }

    
    public virtual void RemoveTarget(Character target)
    {
        targetsList.Remove(target);
        this.target = null;
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
    //------------------------------------------------------------------
    public virtual void InstantiateItem(int id, ShopType type)
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
            case ShopType.Accessory:
                ActiveShield(id);
                break;
            default:
                break;
        }

    }
    //---------------------------------------------------------------
    public void ActiveShield(int id)
    {
        DeActiveShield();
        if (id > 0)
        {
            listShields[id - 1].SetActive(true);
        }
    }
    public void DeActiveShield()
    {
        foreach (GameObject shield in listShields)
        {
            shield.SetActive(false);
        }
    }
    //----------------------------------------------------------------

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
    public Character GetTargetInRange()
    {
        Character target = null;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < targetsList.Count; i++)
        {
            if (Vector3.Distance(TF.position, targetsList[i].TF.position) > Range * size + targetsList[i].size + 2f)
            {
                //Debug.Log("aaaabbb");
                RemoveTarget(targetsList[i]);
                return null;
            }
            if (targetsList[i] != null && targetsList[i] != this && 
                !targetsList[i].IsDead && targetsList[i] != isDespawn) 
            {
                float dis = Vector3.Distance(TF.position, targetsList[i].TF.position);

                if (dis < distance)
                {
                    distance = dis;
                    target = targetsList[i];
                }
            }
        }
        return target;
    }
    public virtual void OnAttack()
    {
        target = GetTargetInRange();
        if (target != null && !target.IsDead)
        {
            targetPoint = target.TF.position;
            // lookat de doi tuong chi xoay theo truc y thay vi xoay ca y,z
            TF.LookAt(targetPoint + (TF.position.y - targetPoint.y) * Vector3.up);
            //TF.forward = (targetPoint - TF.position).normalized;
            //ChangeAnim(Constant.ANIM_ATTACK);
        }

    }
    public void SetTargetMark(bool active)
    {
        targetMark.SetActive(active);
    }
    
    public virtual void OnDead()
    {
        isDespawn = true;

        target = null;
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
        targetsList.Clear();
        //shootPoint.DespawnBullet();
        //ClearListEnemyInAttackRange();
        SimplePool.Despawn(targetIndicator);
        //targetIndicator = null;

    }
    public void IncreaseHP(int aa)
    {
        hp += aa;
    }

    public virtual void SetOwnerForBullet()
    {
        shootPoint.SetOwner(this);
    }

    //-----------------------------------------tang kich thuoc ---------------
    public virtual void AddScore(int value )
    {
        SetScore(score + value);
        UpSize();
    }
    public virtual void SetScore(int score)
    {
        //this.score = score > 0 ? score : 0;
        this.score = score ;

        targetIndicator.SetScore(this.score);
        SetSize(1 + this.score * 0.1f);
    }

    //thay doi kich thuoc
    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, 1f, 5f);
        this.size = size;
        TF.localScale = size * Vector3.one;
    }
    protected virtual void UpSize()
    {
        SetSize(1 + this.score * 0.1f);
    }
}
