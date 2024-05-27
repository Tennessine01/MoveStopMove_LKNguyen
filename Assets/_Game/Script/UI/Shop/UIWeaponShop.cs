using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponShop : UICanvas
{
    public void BackButton()
    {
        Close(0);
        UIManager.Ins.OpenUI<UIMainMenu>();
    }
}
