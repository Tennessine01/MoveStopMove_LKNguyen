using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI playerCoinTxt;

    protected override void OnInit()
    {
        base.OnInit();

    }
    public override void Open()
    {
        base.Open();
    
        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());
    }
    public void PlayButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        Close(0);
    }

    public void SkinShopButton() 
    {
        LevelManager.Ins.SetCameraShop();
        Close(0);
        UIManager.Ins.OpenUI<UISkinShop>();
    }
    public void WeaponShopButton()
    {
        Close(0);
        UIManager.Ins.OpenUI<UIWeaponShop>();
    }
}
