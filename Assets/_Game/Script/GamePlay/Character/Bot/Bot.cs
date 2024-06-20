using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Bot : Character
{
   
    private IState<Bot> currentState;

    public NavMeshAgent agent;
    public float radiusSphere; //radius of sphere

    public Transform centerPoint; //centre of the area the agent wants to move around in
    //instead of centerPoint you can set it as the transform of the agent if you don't care about a specific area

    public Level currentlevel;
    private bool IsCanRunning => (GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting));

    public override void OnInit()
    {

        base.OnInit();

        //ClearListEnemyInAttackRange();
        InstantiateTargetIndicator();
        //ResetItem();
        isPlayer = false;
        isDespawn = false;
        InstantiateWeapon(weaponID);
        InstantiateHat(hatID);
        InstantiatePant(pantID);
        ChangeState(new MenuState());

        LevelManager.Ins.WhenPlayerDie += ChangeMenuState;
        characterName = NameUtilities.GetRandomName();
        targetIndicator.SetName(characterName);

    }
    public override void InstantiateTargetIndicator()
    {
        base.InstantiateTargetIndicator(); 
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(targetIndicator);
        SimplePool.Despawn(this);
        isDespawn = true;
        size = 1;
        ResetItem();
        //Debug.Log("bbbb");
        shootPoint.DespawnBullet();

        ChangeState(new MenuState());
        LevelManager.Ins.WhenPlayerDie -= ChangeMenuState;
    }

    public override void Update()
    {
        if (IsCanRunning && currentState != null)
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
    private void ChangeMenuState()
    {
        if(isDespawn == false)
        {
            agent.isStopped = true;

        }
        ChangeState(new MenuState());
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
    
    public override void AddTarget(Character target)
    {
        base.AddTarget(target);

        if (!IsDead && IsCanRunning)
        {
            ChangeState(new AttackState());
        }
    }
    public override void OnAttack()
    {
        base.OnAttack(); 
    }
    public override void OnDead()
    {
        base.OnDead();
        SimplePool.Despawn(this.targetIndicator);
        //OnDespawn();
        //if (attackRange.characterList.Count > 0)
        //{
        //    attackRange.characterList.Clear();
        //}
        //shootPoint.DespawnBullet();
        //LevelManager.Ins.ReduceListBotNumber(this);
        LevelManager.Ins.EnemyDied(this);
        ChangeState(new DeathState());
    }
}
