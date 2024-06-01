using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI playerCoinTxt;

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
        GameManager.Ins.ChangeState(GameState.SkinShop);
        //LevelManager.Ins.SetCameraShop();
        Close(0);
        //UIManager.Ins.OpenUI<UISkinShop>();
    }
    public void WeaponShopButton()
    {
        GameManager.Ins.ChangeState(GameState.WeaponShop);

        //LevelManager.Ins.DeactivatePlayer();    
        //UIManager.Ins.OpenUI<UIWeaponShop>();
        Close(0);
    }
}
