using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.agent.isStopped = false;
        t.slotWeaponInHand.SetActive(true);
        t.ChangeAnim(Constant.ANIM_RUN);
    }
    public void OnExecute(Bot t)
    {
        if (t.agent.remainingDistance <= t.agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (t.RandomPoint(t.centerPoint.position, t.radiusSphere, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); 
                t.agent.SetDestination(point);
                
            }
            
        }
    }

    public void OnExit(Bot t)
    {

    }
}
