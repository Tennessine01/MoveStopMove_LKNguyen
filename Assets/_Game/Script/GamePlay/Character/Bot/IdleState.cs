using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Bot>
{
    private float timeSpentInState = 0f;
    private float delayToChangeState = 1f; //time to delay

    public void OnEnter(Bot t)
    {
        t.slotWeaponInHand.SetActive(true);    
        t.ChangeAnim(Constant.ANIM_IDLE);
        timeSpentInState = 0f; // reset time counting
    }

    public void OnExecute(Bot t)
    {
        timeSpentInState += Time.deltaTime; // update time each frame

        if (timeSpentInState >= delayToChangeState) // check time 
        {
            t.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot t)
    {
        
    }
}
