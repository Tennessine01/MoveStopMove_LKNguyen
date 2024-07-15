using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    public TMP_Text reward;

    public override void Open()
    {
        base.Open();
        ShowReward();
    }
    public void ShowReward()
    {
        reward.text = (LevelManager.Ins.currentLevel.maxBot*10).ToString();
        LevelManager.Ins.AddCoinWhenWin();
        if (UserDataManager.Ins.userData.currentLevel < LevelManager.Ins.levels.Count - 1)
        {
            UserDataManager.Ins.userData.currentLevel += 1;
        }
    }
    public void MainMenuButton()
    {
        LevelManager.Ins.OnDespawn();

        GameManager.Ins.ChangeState(GameState.MainMenu);
        //UIManager.Ins.OpenUI<MainMenuHome>();
        Close(0);
    }
}
