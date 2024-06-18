using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //tao level
    [SerializeField] public List<Level> levels;
    public Level currentLevel;
    private int levelCount;
    //tao character
    [SerializeField] Player playerPrefab;
    public Player player = null;
    private Bot b;
    private List<Bot> listBot = new List<Bot>(); 
    //bat trong khi play
    [SerializeField] FloatingJoystick joystick;
    //[SerializeField] GameObject targetIndicator;
    [SerializeField] UserDataManager userDataManager;
    //
    private int numberOfCharacter = 0;
    public int BotNumber => listBot.Count;

    public string currentKiller;
    public string currentVictim;
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
        //Debug.Log(userDataManager.userData.currentLevel + "=======================");
        OnLoadLevel(userDataManager.userData.currentLevel);
        SpawnPlayer();
        CameraFollow.Ins.OnInit();        
        OnMenu();
        SpawnInitalBot();
        
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
            numberOfCharacter = newLevel.maxBot;
            levelCount = level;
        }
        if(level != levelCount)
        {
            Destroy(currentLevel);
            Level newLevel = Instantiate(levels[level],transform);
            currentLevel = newLevel;
            numberOfCharacter = newLevel.maxBot;
            levelCount = level;
        }
    }

    //-----------------------------------------------------------------------------------------
    //
    private void SpawnInitalBot()
    {
        for (int i = 0; i < currentLevel.realBot; i++)
        {
            SpawnBot();
        }    
    }
    private void SpawnBot()
    {
        b = SimplePool.Spawn<Bot>(PoolType.Bot, RandomPosition(), Quaternion.identity);
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

        b.OnInit();
        listBot.Add(b);
        if (GameManager.Ins.IsState(GameState.GamePlay))
        {
            b.ChangeState(new IdleState());
        }
    }
    public void SpawnPlayer()
    {
        //Debug.Log("---------------");
        if (player != null)
        {
            //Debug.Log("aaaaaaaaa");
            ActivatePlayer();
            player.ResetItem();
            player.startPos = currentLevel.centerPosition;
            player.TF.forward = new Vector3(0, 0, -90);
            player.OnInit();
        }
        else
        {
            //Debug.Log("bbbbbbbbb");

            Player character = Instantiate(playerPrefab, currentLevel.centerPosition);
            character.startPos = currentLevel.centerPosition;
            character.joystick = joystick;
            player = character;
            player.OnInit();
            CameraFollow.Ins.target = player.TF;
        }

        //Debug.Log(player.startPos.position);
    }
    //---------------------------------------------------------------------------
    public void OnMenu()
    {
        //SetCameraMenu();
        //targetIndicator.SetActive(false);
    }
    public void OnPlay()
    {
        for (int i = 0; i < listBot.Count; i++)
        {
            Bot bot = listBot[i];
            bot.ChangeState(new IdleState());
        }
        //targetIndicator.SetActive(true);
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

    public event Action OnDespawnLevel;
    public void OnDespawn()
    { 
        OnDespawnLevel?.Invoke();
        DespawnPlayer();
        DespawnBot();
    }
    public void DespawnBot()
    {
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
            player.OnDespawn();
            DeactivatePlayer();
            //player.OnStop();
           
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

    public void EnemyDied(Bot bot)
    {
        OnAlivePlayerNumberChanged?.Invoke();
        ReduceListBotNumber(bot);
        if (numberOfCharacter > listBot.Count)
        {
            SpawnBot();
        }
    }
    //------------------------------------
    public int AlivePlayerNumber()
    {
        return numberOfCharacter + 1;
    }
    public event Action MinusNumberOfCharacterOnGround;
    private void ReduceListBotNumber(Bot bot)
    {
        numberOfCharacter -= 1;
        listBot.Remove(bot);   
        MinusNumberOfCharacterOnGround?.Invoke();
        if(listBot.Count == 0)
        {
            GameManager.Ins.ChangeState(GameState.Win);
        }
    }
    public void SetNameKillerAndVictim(string killer, string victim)
    {
        currentKiller = killer;
        currentVictim = victim;
    }    
    //-------------------------------
    public event Action WhenPlayerDie;
    public void PlayerDie()
    {
        WhenPlayerDie?.Invoke();
        GameManager.Ins.ChangeState(GameState.Question);
    }

    //--------------------------------
    public void AddCoinWhenLose()
    {
        UserDataManager.Ins.userData.coin += (currentLevel.realBot - listBot.Count) * 10;
    }
    public string NumberCoinWhenLose()
    {
        return ((currentLevel.realBot - listBot.Count) * 10).ToString();
    }
    public void AddCoinWhenWin()
    {
        UserDataManager.Ins.userData.coin += currentLevel.GetMaxCoin();
    }

    public void SetTargetIndicatorAlpha(float alpha)
    {
        HashSet<GameUnit> list = SimplePool.GetAllUnitIsActive(PoolType.TargetIndicator);

        foreach (GameUnit unit in list)
        {
            (unit as TargetIndicator).SetAlpha(alpha);
        }
    }

    //public void OnCharacterDead(string killer, string victim)
    //{
    //    UIManager.Ins.GetUI<UIGamePlay>().OnShowTagName(killer, victim);
    //}
}