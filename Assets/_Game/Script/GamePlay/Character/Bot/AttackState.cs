using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    private float timeSpentInState = 0f;
    private float delayToChangeState = 0.4f; //time to delay
    public void OnEnter(Bot t)
    {
        t.agent.isStopped = true;
        t.ChangeAnim(Constant.ANIM_ATTACK);
    }

    public void OnExecute(Bot t)
    {
        timeSpentInState += Time.deltaTime; // update time each frame
        //t.CheckClosestEnemy();
        t.OnAttack();
        //t.ChangeState(new  IdleState());
        if (timeSpentInState >= delayToChangeState) // check time 
        {
            t.slotWeaponInHand.SetActive(false);
            t.shootPoint.Shoot(t.weapon.bulletType, t.size);
            t.ChangeState(new IdleState());
        }     
    }

    public void OnExit(Bot t)
    {

    }

}
