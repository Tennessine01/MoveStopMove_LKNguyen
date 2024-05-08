using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Level[] levels;
    public Level currentLevel;

    public void Start()
    {
        OnLoadLevel(0);
        OnInit();
    }

    //khoi tao trang thai bat dau game
    public void OnInit()
    {
        //Debug.Log(currentLevel.realBot);
        //player.OnInit();
        for (int i = 0; i < currentLevel.realBot; i++)
        {
            //Debug.Log(i + "-----");
            SpawnBot();
        }
    }

    //reset trang thai khi ket thuc game
    public void OnReset()
    {
        //player.OnDespawn();
        //for (int i = 0; i < bots.Count; i++)
        //{
        //    bots[i].OnDespawn();
        //}

        //bots.Clear();
        //SimplePool.CollectAll();
    }

    //tao prefab level moi
    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[level]);
    }

    //-----------------------------------------------------------------------------------------
    private void SpawnBot()
    {
        Bot b = (Bot)SimplePool.Spawn<GameUnit>(PoolType.Bot, RandomPosition(), Quaternion.identity);
        b.centerPoint = currentLevel.centerPosition;
    }

    private Vector3 RandomPosition()
    {
        Vector3 randomPos = new Vector3();
        randomPos = currentLevel.RandomPoint();
        return randomPos;
    }
}