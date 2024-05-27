using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public enum GameState { MainMenu, GamePlay, Finish, Revive, Setting }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private static GameState gameState = GameState.MainMenu;

    // Start is called before the first frame update
    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        //csv.OnInit();
        //userData?.OnInitData();

        
    }
    public void Start()
    {
        ChangeState(GameState.MainMenu);
        //LevelManager.Ins.OnInit();

        UIManager.Ins.OpenUI<UIMainMenu>();
    }
    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.MainMenu:
                ChangeMainMenuState();
                break;
            case GameState.GamePlay:
                ChangeGamePlayState();
                break;
            case GameState.Finish:
                ChangeFinishState();
                break;
            case GameState.Revive:
                break;
            case GameState.Setting:
                break;
            default:
                break;
        }
    }

    private void ChangeFinishState()
    {
    }

    private void ChangeMainMenuState()
    {
        //LevelManager.Ins.OnInit();
        LevelManager.Ins.SetCameraMenu();
    }

    private void ChangeGamePlayState()
    {
        LevelManager.Ins.SetCameraFollow();
        LevelManager.Ins.OnPlay();
        UIManager.Ins.OpenUI<UIGamePlay>();
    }

    public static bool IsState(GameState state)
    {
        return gameState == state;
    }

}
