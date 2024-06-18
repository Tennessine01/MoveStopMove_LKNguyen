using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : UICanvas
{
    public void LoseButton()
    {
        //LevelManager.Ins.OnDespawn();

        GameManager.Ins.ChangeState(GameState.Lose);
        Close(0);
    }
    public void ReviveButton()
    {
        GameManager.Ins.ChangeState(GameState.Revive);    
        Close(0);
    }
}