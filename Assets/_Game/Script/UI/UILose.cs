using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    public TMP_Text ranking;  

    public override void Open()
    {
        base.Open();
        ranking.text ="#"+ LevelManager.Ins.AlivePlayerNumber().ToString();
    }
    public void MainMenuButton()
    {
        LevelManager.Ins.OnDespawn();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }
}
