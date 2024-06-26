using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class Player : Character
{
    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Setting);

    //coin
    public int coin;

    public FloatingJoystick joystick;
    public Transform startPos;

    private bool isMoving;
    public bool isAttack = true;
    private bool wasMovingLastFrame = false;
    private Vector3 movementDirection;
    [SerializeField] private int reviveTime;
    public override void OnInit()
    {
        OnLoadData();

        base.OnInit();
        InstantiateWeapon(weaponID);
        ActiveHats(hatID);
        InstantiatePant(pantID);
        ActiveShield(shieldID);
        isMoving = false;
        isAttack = true;
        isPlayer = true;
        isDespawn = false;
        TF.position = startPos.position;
        InstantiateTargetIndicator();
        reviveTime = 1;
        characterName = "Nguyen";
        targetIndicator.SetName(characterName);

    }
    public override void Update()
    {
        if (IsCanUpdate && !IsDead)
        {
            if (isDespawn == true)
            {
                return;
            }
            JoystickMove();
            //CheckClosestEnemy();
        }
    }
    public override void SetOwnerForBullet()
    {
        //Debug.Log("fffffff");
        shootPoint.SetOwner(this);
    }
    public override void SetScore(int score)
    {
        base.SetScore(score);
    }
    //-------------------------------------------------------------------
    public void OnLoadData()
    {
        //if (UserDataManager.Ins.userData.currentWeapon == 0)
        //{
        //    weaponID = 0;
        //}
        //else
        //{
        //    weaponID = UserDataManager.Ins.userData.currentWeapon;
        //}
        weaponID = UserDataManager.Ins.userData.currentWeapon;
        shieldID = UserDataManager.Ins.userData.currentAccessory;
        hatID = UserDataManager.Ins.userData.currentHat;
        pantID = UserDataManager.Ins.userData.currentPant;
        coin = UserDataManager.Ins.userData.coin;
    }
    private void JoystickMove()
    {

        movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized; //chuan hoa ve unit vector
        if (movementDirection.magnitude > 0.1f)
        {
            StopAllCoroutines();
            isMoving = true;
            isAttack = true;
            slotWeaponInHand.SetActive(true);

            //transform.position += movementDirection * Time.deltaTime * moveSpeed;
            rb.velocity = movementDirection * moveSpeed;

            TF.forward = movementDirection;

            ChangeAnim(Constant.ANIM_RUN);
        }
        if (movementDirection.magnitude < 0.1f)
        {
            isMoving = false;
            //if (target == null)
            //{
            //    slotWeaponInHand.SetActive(true);

            //    isAttack = false;
            //}
            rb.velocity = Vector3.zero;

           
            ChangeAnim(Constant.ANIM_IDLE);
            OnAttack();

            if (isDespawn == true)
            {
                ChangeAnim(Constant.ANIM_DEAD);
            }
        }
        
    }
    //public override void AttackWhenStop()
    //{
    //    base.AttackWhenStop();
    //    if (attackRange.targetCharacter != null)
    //    {
    //        //quay ve huong ke dich
    //        //TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;

    //        isAttack = true;
    //        Attack();
    //    }
    //}
    public override void AddTarget(Character target)
    {
        base.AddTarget(target);

        if (!target.IsDead && !IsDead)
        {
            target.SetTargetMark(true);
            if (isMoving == false && isAttack == true)
            {
                OnAttack();
            }
        }
    }
    public override void RemoveTarget(Character target)
    {
        base.RemoveTarget(target);
        target.SetTargetMark(false);
    }
    public override void OnAttack()
    {
        base.OnAttack();
        if (target != null && !target.IsDead)
        {
            ChangeAnim(Constant.ANIM_ATTACK);
        }
        if (target != null && isMoving == false && isAttack == true && !target.IsDead)
        {
            Attack();
            //isAttack = false;
        }
    }
    public void Attack()
    {
        if (isAttack == true)
        {
            StartCoroutine(CheckAttackFalse());
        }
    }
    IEnumerator CheckAttackFalse()
    {
        isAttack = false;
        //Debug.Log("----");
        // 
        //ChangeAnim(Constant.ANIM_ATTACK);
        yield return new WaitForSeconds(0.4f);
        slotWeaponInHand.SetActive(false);
        //Debug.Log("shoottttt");
        shootPoint.Shoot(weapon.bulletType, size);
        isAttack = false;
        //Debug.Log(isAttack);

        //yield return new WaitForSeconds(0.1f);
        //isAttack = false;
        yield return new WaitForSeconds(0.1f);
        slotWeaponInHand.SetActive(true);
    }
    
    public void OnRevive()
    {
        //InstantiateTargetIndicator();
        //targetIndicator.SetName("Nguyen");
        reviveTime--;
        isDespawn = false;
        joystick.gameObject.SetActive(true);
        IncreaseHP(10);
        //SetScore(score);
    }
    private void CheckReviveTime()
    {
        if (reviveTime > 0)
        {
            LevelManager.Ins.PlayerDie();
        }
        if (reviveTime == 0)
        {
            ChangeAnim(Constant.ANIM_DEAD);
            GameManager.Ins.ChangeState(GameState.Lose);
        }
    }
    
    public override void OnDead()
    {
        base.OnDead();
        //SimplePool.Despawn(targetIndicator);

        //joystick.gameObject.SetActive(false);
        ChangeAnim(Constant.ANIM_DEAD);
        OnStop();
        CheckReviveTime();
    }
    public void OnStop()
    {
        rb.velocity = Vector3.zero;
        ChangeAnim(Constant.ANIM_IDLE);
    }
    public override void OnDespawn() 
    { 
        base.OnDespawn();
        SimplePool.Despawn(targetIndicator);
        OnStop();
    }

    public override void InstantiateItem(int id, ShopType type)
    {
        switch (type)
        {
            case ShopType.Hat:
                ActiveHats(id);
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

    public void ActiveHats(int id)
    {
        DeActiveHats();
        if (id > 0)
        {
            listHats[id - 1].SetActive(true);
        }
    }
    public void DeActiveHats()
    {
        foreach (GameObject shield in listHats)
        {
            shield.SetActive(false);
        }
    }
}