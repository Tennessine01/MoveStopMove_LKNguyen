using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UICanvas
{
    public TMP_Text reward;

    public override void Open()
    {
        base.Open();
        reward.text = LevelManager.Ins.currentLevel.GetMaxCoin().ToString();
        UserDataManager.Ins.userData.coin += LevelManager.Ins.currentLevel.GetMaxCoin() ;

    }
    public void MainMenuButton()
    {
        LevelManager.Ins.OnDespawn();

        GameManager.Ins.ChangeState(GameState.MainMenu);
        //UIManager.Ins.OpenUI<UIMainMenu>();
        Close(0);
    }
}
