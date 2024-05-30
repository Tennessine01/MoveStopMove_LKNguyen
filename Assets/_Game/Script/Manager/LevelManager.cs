using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //tao level
    [SerializeField] List<Level> levels;
    public Level currentLevel;
    private int levelCount;
    //tao character
    [SerializeField] Player playerPrefab;
    public Player player = null;
    private Bot b;
    private List<Bot> listBot = new List<Bot>(); 
    //bat trong khi play
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] GameObject targetIndicator;

    //
    public int BotNumber => listBot.Count;
    //public void Start()
    //{
    //    OnInit();
    //}

    //private void Awake()
    //{
    //    OnInit();
    //}
    public void OnInit()
    {
        if(listBot != null)
        {
            for (int i = 0; i < listBot.Count; i++)
            {
                Bot bot = listBot[i];
                //bot.shootPoint.b.OnDespawn();
                bot.OnDespawn();
                SimplePool.Despawn(bot);
            }
            listBot.Clear();
        }
        OnLoadLevel(0);
        SpawnPlayer();
        CameraFollow.Ins.OnInit();        
        OnMenu();
        SpawnBot();
        
    }

    //reset trang thai khi ket thuc game
    public void OnReset()
    {
        //player.OnDead();
        //for (int i = 0; i < bots.Count; i++)
        //{
        //    bots[i].OnDead();
        //}

        //bots.Clear();
        //SimplePool.CollectAll();
    }

    //tao prefab level moi
    public void OnLoadLevel(int level)
    {
        if(currentLevel == null)
        {
            Level newLevel = Instantiate(levels[level], transform);
            currentLevel = newLevel;
            levelCount = level;
        }
        if(level != levelCount)
        {
            Level newLevel = Instantiate(levels[level],transform);
            currentLevel = newLevel;
            levelCount = level;

        }
    }

    //-----------------------------------------------------------------------------------------
    //
    private void SpawnBot()
    {
        for (int i = 0; i < currentLevel.realBot; i++)
        {
            b = SimplePool.Spawn<Bot>(PoolType.Bot, RandomPosition(), Quaternion.identity);
            b.OnInit();
            b.centerPoint = currentLevel.centerPosition;

            //random weapon
            int randomWeaponID = UnityEngine.Random.Range(0, 6);
            b.weaponID = randomWeaponID;

            //random hatPrefab
            int randomHatID = UnityEngine.Random.Range(1, 9);
            b.hatID = randomHatID;

            //random pant
            int pantID = UnityEngine.Random.Range(1, 7);
            b.pantID = pantID;

            listBot.Add(b);
        }
            
    }
    public void SpawnPlayer()
    {
        Debug.Log("---------------");
        if (player != null)
        {
            ActivatePlayer();
            player.ResetItem();
            player.OnInit();
        }
        else
        {
            Player character = Instantiate(playerPrefab, currentLevel.centerPosition);
            character.startPos = currentLevel.centerPosition;
            character.joystick = joystick;
            player = character;
            CameraFollow.Ins.target = player.TF;
        }

        //Debug.Log(player.startPos.position);
    }
    //---------------------------------------------------------------------------
    public void OnMenu()
    {
        //SetCameraMenu();
        targetIndicator.SetActive(false);
    }
    public void OnPlay()
    {
        for (int i = 0; i < listBot.Count; i++)
        {
            Bot bot = listBot[i];
            bot.ChangeState(new IdleState());
        }
        targetIndicator.SetActive(true);
    }
    //-------------------------------------------------------------------
    public void SetCameraMenu()
    {
        CameraFollow.Ins.SetOffset(0, 5, -8);
        CameraFollow.Ins.SetRotation(30f, 0, 0);
    }
    public void SetCameraFollow()
    {
        CameraFollow.Ins.SetOffset(0,22.9f,-14.7f);
        CameraFollow.Ins.SetRotation(55, 0, 0);
        //meraFollow.target = player.transform;
    }
    public void SetCameraShop()
    {
        CameraFollow.Ins.SetOffset(0, 3f, -8);
        CameraFollow.Ins.SetRotation(30f, 0, 0);
    }

    //--------------------------------------------------------------------------
    private Vector3 RandomPosition()
    {
        Vector3 randomPos = new Vector3();
        randomPos = currentLevel.RandomPoint();
        return randomPos;
    }

    public void OnDespawn()
    {
        if(player != null)
        {
            DespawnPlayer();
        }
        if (listBot != null)
        {
            for (int i = 0; i < listBot.Count; i++)
            {
                Bot bot = listBot[i];
                //bot.shootPoint.b.OnDespawn();
                bot.OnDespawn();
                SimplePool.Despawn(bot);
            }
            listBot.Clear();
        }
    }
    public void DespawnPlayer()
    {
        if (player != null)
        {
            //Destroy(player);
            DeactivatePlayer();
            player.ResetItem();
        }
    }
    public void DeactivatePlayer()
    {
        if (player != null)
        {
            player.gameObject.SetActive(false);
        }
    }
    public void ActivatePlayer()
    {
        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.OnInit();
        }
    }

    //------------------------------
    public event Action OnAlivePlayerNumberChanged;

    public void EnemyDied()
    {
        OnAlivePlayerNumberChanged?.Invoke();
    }
    public int AlivePlayerNumber()
    {
        return listBot.Count + 1;
    }
    public void ReduceListBotNumber(Bot bot)
    {
        if (bot.isDespawn == true)
        {
            listBot.Remove(bot);
        }
    }

    //-------------------------------
    public event Action WhenPlayerDie;
    public void PlayerDie()
    {
        WhenPlayerDie?.Invoke();
        GameManager.Ins.ChangeState(GameState.Question);
    }
}