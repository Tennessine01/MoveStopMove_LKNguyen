using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGamePlay : UICanvas
{
    [SerializeField] public TextMeshProUGUI aliveNumber;

    public override void Open()
    {
        base.Open();
        ShowAlivePlayer();
        LevelManager.Ins.OnAlivePlayerNumberChanged += ShowAlivePlayer;

    }
    public void ShowAlivePlayer()
    {
        aliveNumber.text = LevelManager.Ins.AlivePlayerNumber().ToString();
    }
    public void SettingButton()
    {
        GameManager.Ins.ChangeState(GameState.Setting);
    }

    public override void CloseDirectly()
    {
        if (LevelManager.Ins != null)
        {
            LevelManager.Ins.OnAlivePlayerNumberChanged -= ShowAlivePlayer;
        }

        base.CloseDirectly();
    }
}
