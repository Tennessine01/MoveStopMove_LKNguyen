﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public enum GameState { MainMenu, GamePlay, Win, Revive,Question, Setting , Lose , SkinShop, WeaponShop}

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private GameState gameState;

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

    }
    public void ChangeState(GameState state)
    {
        this.gameState = state;
        switch (gameState)
        {
            case GameState.MainMenu:
                ChangeMainMenuState();
                break;
            case GameState.GamePlay:
                ChangeGamePlayState();
                break;
            case GameState.Win:
                ChangeWinState();
                break;
            case GameState.Revive:
                ChangeReviveState();
                break;
            case GameState.Setting:
                ChangeSettingState();
                break;
            case GameState.Lose:
                ChangeLoseState();
                break;
            case GameState.Question:
                ChangeQuestionState();
                break;
            case GameState.SkinShop:
                ChangeSkinShopState();
                break;
            case GameState.WeaponShop:
                ChangeWeaponShopState();
                break;
            default:
                break;
        }
    }
    private void ChangeSkinShopState()
    {
        UIManager.Ins.OpenUI<UISkinShop>();
        LevelManager.Ins.SetCameraShop();

    }
    private void ChangeWeaponShopState()
    {
        UIManager.Ins.OpenUI<UIWeaponShop>();
        LevelManager.Ins.DeactivatePlayer();

    }
    private void ChangeSettingState()
    {
        UIManager.Ins.OpenUI<UISetting>();
    }
    private void ChangeWinState()
    {
        UIManager.Ins.OpenUI<UIWin>();
        LevelManager.Ins.OnDespawn();
        LevelManager.Ins.OnInit();
    }
    private void ChangeReviveState() 
    {
        LevelManager.Ins.player.OnRevive();
        GameManager.Ins.ChangeState(GameState.GamePlay);
    }
    public void ChangeQuestionState()
    {
        UIManager.Ins.CloseUI<UISetting>();
        UIManager.Ins.OpenUI<UIRevive>();
    }
    private void ChangeMainMenuState()
    {   
        UIManager.Ins.CloseUI<UILose>();
        UIManager.Ins.CloseUI<UIWin>();
        UIManager.Ins.CloseUI<UIGamePlay>();
        UIManager.Ins.CloseUI<UISetting>();

        
        //LevelManager.Ins.OnDespawn();
        
        LevelManager.Ins.OnInit();
        LevelManager.Ins.SetCameraMenu();
        UIManager.Ins.OpenUI<UIMainMenu>();

    }

    private void ChangeGamePlayState()
    {
        LevelManager.Ins.SetCameraFollow();
        LevelManager.Ins.OnPlay();
        UIManager.Ins.OpenUI<UIGamePlay>();
    }
    private void ChangeLoseState()
    {
        UIManager.Ins.CloseUI<UIGamePlay>();
        UIManager.Ins.CloseUI<UISetting>();
        UIManager.Ins.OpenUI<UILose>();
        LevelManager.Ins.OnDespawn();
        LevelManager.Ins.OnInit();
    }
    public bool IsState(GameState state)
    {
        return this.gameState == state;
    }
    
}
