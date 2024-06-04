using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Setting);

    //coin
    public int coin;

    public FloatingJoystick joystick;
    public Transform startPos;

    private bool isMoving;
    public bool isAttack = false;
    private bool wasMovingLastFrame = false;
    private Vector3 movementDirection;
    [SerializeField] private int reviveTime;
    public override void OnInit()
    {
        OnLoadData();

        base.OnInit();
        InstantiateWeapon(weaponID);
        InstantiateHat(hatID);
        InstantiatePant(pantID);
        isMoving = false;
        isAttack = false;
        isPlayer = true;
        isDespawn = false;
        TF.position = startPos.position;
        //if (joystick != null)
        //{
        //    joystick.gameObject.SetActive(true);
        //}
        reviveTime = 1;
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
            CheckClosestEnemy();
            if (wasMovingLastFrame == true && isMoving == false)
            {
                AttackWhenStop();
            }

            wasMovingLastFrame = isMoving;

        }
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
            isAttack = false;
            //transform.position += movementDirection * Time.deltaTime * moveSpeed;
            rb.velocity = movementDirection * moveSpeed;

            TF.forward = movementDirection;

            ChangeAnim(Constant.ANIM_RUN);
        }
        if (movementDirection.magnitude < 0.1f)
        {
            isMoving = false;
            if (attackRange.targetCharacter == null)
            {
                slotWeaponInHand.SetActive(true);

                isAttack = false;
            }
            rb.velocity = Vector3.zero;

            if (isAttack == false)
            {

                ChangeAnim(Constant.ANIM_IDLE);
            }

            if (isDespawn == true)
            {
                ChangeAnim(Constant.ANIM_DEAD);
            }
        }
        
    }
    public override void AttackWhenStop()
    {
        base.AttackWhenStop();
        if (attackRange.targetCharacter != null)
        {
            //quay ve huong ke dich
            //TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;

            isAttack = true;
            Attack();
        }
    }
    public void Attack()
    {
        StartCoroutine(CheckAttackFalse());
    }
    IEnumerator CheckAttackFalse()
    {
        ChangeAnim(Constant.ANIM_ATTACK);
        yield return new WaitForSeconds(0.4f);
        slotWeaponInHand.SetActive(false);
        shootPoint.Shoot(weapon.bulletType);
        yield return new WaitForSeconds(0.1f);
        isAttack = false;
        yield return new WaitForSeconds(0.1f);
        slotWeaponInHand.SetActive(true);
    }
    public void OnRevive()
    {
        reviveTime--;
        isDespawn = false;
        joystick.gameObject.SetActive(true);
        IncreaseHP(10);
    }
    private void CheckReviveTime()
    {
        if (reviveTime > 0)
        {
            LevelManager.Ins.PlayerDie();
        }
        if (reviveTime == 0)
        {
            GameManager.Ins.ChangeState(GameState.Lose);
        }
    }
    public override void OnDead()
    {
        base.OnDead();
        //joystick.gameObject.SetActive(false);
        ChangeAnim(Constant.ANIM_DEAD);
        OnStop();
        CheckReviveTime();
    }
    public void OnStop()
    {
        rb.velocity = Vector3.zero;
    }
    public override void OnDespawn() 
    { 
        base.OnDespawn();
        OnStop();
    }

}