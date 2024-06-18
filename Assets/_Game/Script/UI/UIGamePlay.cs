using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class UIGamePlay : UICanvas
{
    [SerializeField] public TextMeshProUGUI aliveNumber;
    [SerializeField] public Transform container;
    [SerializeField] TagNameItem tagNameItemPrefab;
    private List<TagNameItem> tagNameItemList = new();
    public override void Open()
    {
        base.Open();
        ShowAlivePlayer();
        LevelManager.Ins.SetTargetIndicatorAlpha(1);

        LevelManager.Ins.MinusNumberOfCharacterOnGround += ShowAlivePlayer;
        LevelManager.Ins.OnAlivePlayerNumberChanged += ShowTagNameWhenKill;

    }
    public void ShowTagNameWhenKill()
    {
        //TagNameItem tagNameItem = Instantiate(tagNameItemPrefab, container);
        TagNameItem tagNameItem = SimplePool.Spawn<TagNameItem>(PoolType.TagName, container.position, Quaternion.identity);
        tagNameItem.transform.SetParent(container);
        tagNameItem.SetNameWhenKill(LevelManager.Ins.currentKiller, LevelManager.Ins.currentVictim);
        tagNameItem.OnInit();
        tagNameItemList.Add(tagNameItem);
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
            LevelManager.Ins.MinusNumberOfCharacterOnGround -= ShowAlivePlayer;
            LevelManager.Ins.OnAlivePlayerNumberChanged -= ShowTagNameWhenKill;

        }
        LevelManager.Ins.SetTargetIndicatorAlpha(0);
        foreach (var item in tagNameItemList)
        {
            item.OnDespawn();
            //SimplePool.Despawn(item);
        }
        base.CloseDirectly();
    }

    //internal void OnShowTagName(string killer, string victim)
    //{
    //    throw new NotImplementedException();
    //}
}
