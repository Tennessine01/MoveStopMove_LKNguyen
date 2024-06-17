using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class Character : GameUnit
{
    [Header("Character Attributes")]

    ////Range
    [SerializeField] public float Range = 6f;
    //Movespeed
    [SerializeField] public float moveSpeed;
    ////AttackSpeed
    //[SerializeField] public float speed;
    ////Size
    [SerializeField] public float size = 1;
    private int score;
    public int Score => score;
    //Skin
    [SerializeField] public GameObject targetMark;

    //Name
    public string characterName;

    [SerializeField] Transform indicatorPosition;
    protected TargetIndicator targetIndicator;
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
    public List<Character> targetsList = new List<Character>();
    public Character target;
    //vi tri cua muc tieu
    private Vector3 targetPoint;

    public virtual void OnInit()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        slotWeaponInHand.SetActive(true);
        target = null;
        hp = 10;
        size = 1;
        score = 0;
        //attackRange.range = Range;
        TF.localScale = new Vector3(1, 1, 1)*size;
        //sinh vu khi
        //InstantiateWeapon();
        SetOwnerForBullet();
        attackRange.transform.localScale = new Vector3(1, 1, 1) * Range;
        //tao target indicator
        InstantiateTargetIndicator();
        //
    }
    public void InstantiateTargetIndicator()
    {
        targetIndicator = SimplePool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        targetIndicator.SetTarget(indicatorPosition);
    }
    // Update is called once per frame
    public virtual void Update()
    {


    }

    //them muc tieu
    public virtual void AddTarget(Character target)
    {
        targetsList.Add(target);
    }

    //xoas muc tieu
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
  
    //public virtual void AttackWhenStop()
    //{
    //    if (attackRange.targetCharacter != null)
    //    {
    //        if (attackRange.targetCharacter.isDespawn == true)
    //        {
    //            //Debug.Log("gggggggggggggggg");
    //            attackRange.characterList.Remove(attackRange.targetCharacter);
    //            attackRange.targetCharacter = null;
    //        }
    //        else{
    //            //quay ve huong ke dich
    //            TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;
    //        }
    //    }
    //}
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
    //----------------------------------------------------------------
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
    public Character GetTargetInRange()
    {
        Character target = null;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < targetsList.Count; i++)
        {
            if (targetsList[i] != null && targetsList[i] != this && !targetsList[i].IsDead && targetsList[i] != isDespawn && Vector3.Distance(TF.position, targetsList[i].TF.position) <= Range * size + targetsList[i].size)
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
            ChangeAnim(Constant.ANIM_ATTACK);
        }

    }
    public void SetTargetMark(bool active)
    {
        targetMark.SetActive(active);
    }
    //public void CheckClosestEnemy()
    //{
    //    attackRange.DetectNearCharacter();
    //}
    public virtual void OnDead()
    {
        isDespawn = true;
        targetsList.Clear();
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
        shootPoint.DespawnBullet();
        //ClearListEnemyInAttackRange();
        SimplePool.Despawn(targetIndicator);

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
    //public void ClearListEnemyInAttackRange()
    //{
        
    //    attackRange.characterList.Clear();
        
        
    //    attackRange.targetCharacter = null;
    //}

    //-----------------------------------------tang kich thuoc ---------------
    public void AddScore(int value )
    {
        SetScore(score + value);
    }
    public void SetScore(int score)
    {
        this.score = score > 0 ? score : 0;
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

}
