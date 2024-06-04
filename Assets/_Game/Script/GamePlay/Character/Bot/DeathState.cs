using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState<Bot>
{
    private float timeSpentInState = 0f;
    private float delayToChangeState = 1.5f; //time to delay
    public void OnEnter(Bot t)
    {
        t.agent.isStopped = true;
        t.ChangeAnim(Constant.ANIM_DEAD);
    }

    public void OnExecute(Bot t)
    {
        timeSpentInState += Time.deltaTime; // update time each frame

        if (timeSpentInState >= delayToChangeState) // check time 
        {
            SimplePool.Despawn(t);
            t.OnDespawn();
        }
    }

    public void OnExit(Bot t)
    {

    }

}
