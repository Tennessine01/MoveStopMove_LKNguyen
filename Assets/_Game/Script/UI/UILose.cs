using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    public TMP_Text ranking;
    public TMP_Text reward;


    public override void Open()
    {
        base.Open();
        LevelManager.Ins.player.ChangeAnim(Constant.ANIM_DEAD);
        ranking.text ="#"+ LevelManager.Ins.AlivePlayerNumber().ToString();
        reward.text = LevelManager.Ins.NumberCoinWhenLose();
        LevelManager.Ins.AddCoinWhenLose();

    }
    public void MainMenuButton()
    {
        LevelManager.Ins.OnDespawn();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }
}
