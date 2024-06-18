using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuHome : UICanvas
{
    private const string ANIM_OPEN = "OpenMenu";
    private const string ANIM_CLOSE = "CloseMenu";
    [SerializeField] TextMeshProUGUI playerCoinTxt;
    [SerializeField] TMP_Text zone;
    [SerializeField] Animation anim;
    public override void Open()
    {
        base.Open(); 
        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());
        zone.SetText((UserDataManager.Ins.userData.currentLevel + 1).ToString());
        anim.Play(ANIM_OPEN);
    }
    public void PlayButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        anim.Play(ANIM_CLOSE);
        Close(0.5f);
    }

    public void SkinShopButton() 
    {
        GameManager.Ins.ChangeState(GameState.SkinShop);
        //LevelManager.Ins.SetCameraShop();
        anim.Play(ANIM_CLOSE);
        Close(0.5f);
        //UIManager.Ins.OpenUI<SkinShop>();
    }
    public void WeaponShopButton()
    {
        GameManager.Ins.ChangeState(GameState.WeaponShop);

        //LevelManager.Ins.DeactivatePlayer();    
        //UIManager.Ins.OpenUI<WeaponShop>();
        Close(0.5f);
        anim.Play(ANIM_CLOSE);
    }
}
