using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;

    

    private bool isMoving;
    public bool isAttack = false;
    private bool wasMovingLastFrame = false;

    public override void OnInit()
    {
        base.OnInit();
        isPlayer = true;
        

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
    private void JoystickMove()
    {
        Vector3 movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (movementDirection.magnitude > 0.1f)
        {
            StopAllCoroutines();
            isMoving = true;
            //transform.position += movementDirection * Time.deltaTime * moveSpeed;
            rb.velocity = movementDirection * moveSpeed;
            //float targetAngle = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg + 90f;
            //Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            //transform.right = targetRotation * Vector3.forward;
            transform.forward = movementDirection;

            ChangeAnim(Constant.ANIM_RUN);
        }
        if (movementDirection.magnitude < 0.1f)
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
            if (isAttack == false)
            {
                ChangeAnim(Constant.ANIM_IDLE);
            }
        }
    }
    public override void AttackWhenStop()
    {

        if (attackRange.targetCharacter != null)
        {

            //quay ve huong ke dich
            transform.forward = (attackRange.targetCharacter.transform.position - transform.position).normalized;

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
        yield return new WaitForSeconds(0.4f);
        slotWeaponInHand.SetActive(true);
    }
    

}