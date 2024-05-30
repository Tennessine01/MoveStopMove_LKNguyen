using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UICanvas
{
    public void ContinueButton()
    {
        Close(0);
    }

    public void HomeButton()
    {
        //LevelManager.Ins.OnDespawn();
        //LevelManager.Ins.OnInit();

        GameManager.Ins.ChangeState(GameState.MainMenu);

        Close(0);
        UIManager.Ins.CloseUI<UIGamePlay>();
    }
    
}
