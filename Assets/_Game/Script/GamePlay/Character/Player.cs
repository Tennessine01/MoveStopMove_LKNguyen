using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    //coin
    public int coin;

    public FloatingJoystick joystick;
    public Transform startPos;

    private bool isMoving;
    public bool isAttack = false;
    private bool wasMovingLastFrame = false;

    public override void OnInit()
    {
        OnLoadData();

        base.OnInit();
        InstantiateWeapon(weaponID);
        InstantiateHat(hatID);
        InstantiatePant(pantID); 
        isPlayer = true;
        TF.position = startPos.position;
    }
    public override void Update()
    {
        JoystickMove();
        CheckClosestEnemy();
        if (wasMovingLastFrame && !isMoving)
        {
            AttackWhenStop();
        }
        
        wasMovingLastFrame = isMoving;
    }

    //-------------------------------------------------------------------
    public void OnLoadData()
    {
        if(UserDataManager.Ins.userData.currentWeapon == 0){
            weaponID = 1;
        }
        else{
            weaponID = UserDataManager.Ins.userData.currentWeapon;
        }
        hatID = UserDataManager.Ins.userData.currentHat;
        pantID = UserDataManager.Ins.userData.currentPant;
        coin = UserDataManager.Ins.userData.coin;
    }
    private void JoystickMove()
    {
        Vector3 movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized; //chuan hoa ve unit vector
        if (movementDirection.magnitude > 0.1f)
        {
            StopAllCoroutines();
            isMoving = true;
            //transform.position += movementDirection * Time.deltaTime * moveSpeed;
            rb.velocity = movementDirection * moveSpeed;
            
            TF.forward = movementDirection;

            ChangeAnim(Constant.ANIM_RUN);
        }
        if (movementDirection.magnitude < 0.1f)
        {
            if (attackRange.targetCharacter == null)
            {
                slotWeaponInHand.SetActive(true);

                isAttack = false;
            }
            isMoving = false;
            rb.velocity = Vector3.zero;
            if (isAttack == false)
            {
                ChangeAnim(Constant.ANIM_IDLE);
            }
        }
    }
    public override void AttackWhenStop(){
        if (attackRange.targetCharacter != null){
            //quay ve huong ke dich
            TF.forward = (attackRange.targetCharacter.TF.position - TF.position).normalized;

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
    
    public void EquipWeapon(int a)
    {
        weaponID = a;
    }
}