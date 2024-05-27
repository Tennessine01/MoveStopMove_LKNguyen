using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Bot : Character
{
    [SerializeField] public Renderer targetMark;

    private IState<Bot> currentState;

    public NavMeshAgent agent;
    public float radiusSphere; //radius of sphere

    public Transform centerPoint; //centre of the area the agent wants to move around in
    //instead of centerPoint you can set it as the transform of the agent if you don't care about a specific area

    public Level currentlevel;

    public override void OnInit()
    {

        base.OnInit();
        isPlayer = false;
        isDespawn = false;
        InstantiateWeapon(weaponID);
        InstantiateHat(hatID);
        InstantiatePant(pantID);
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new MenuState());

    }
    public void OnDespawn()
    {
        isDespawn = true;
        //Debug.Log("bbbb");
        ChangeState(new MenuState());

        if (attackRange.targetCharacter != null)
        {
            attackRange.characterList.Clear();
        }
    }

    public override void Update()
    {
        if (currentState != null)
        {   
            currentState.OnExecute(this);
        }
    }

    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if radiusSphere is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    public override void AttackWhenStop()
    {
        base.AttackWhenStop();
    }

    public override void OnDead()
    {
        isDespawn = true;
        ChangeState(new DeathState());
    }
}
