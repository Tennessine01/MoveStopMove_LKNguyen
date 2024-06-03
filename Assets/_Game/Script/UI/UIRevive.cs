using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRevive : UICanvas
{
    public void MainMenuButton()
    {
        LevelManager.Ins.OnDespawn();

        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }
    public void ReviveButton()
    {
        GameManager.Ins.ChangeState(GameState.Revive);    
        Close(0);
    }
}