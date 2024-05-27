using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : UICanvas
{

    public void SettingButton()
    {
        GameManager.Ins.ChangeState(GameState.Setting);

        UIManager.Ins.OpenUI<UISetting>();
    }
   
}
