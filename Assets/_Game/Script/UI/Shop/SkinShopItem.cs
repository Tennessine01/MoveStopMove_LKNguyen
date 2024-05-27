using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemState { Buy = 0, Bought = 1 , Equipped = 2, Selecting = 3 }


public class SkinShopItem : MonoBehaviour
{
    [SerializeField] GameObject[] stateObjects;

    public Image ImgIcon;
    public ItemState state;
    public UISkinShop uiskinshop;
    public ShopType shopType;
    public int price;
    
    public int id;  
    internal void Setup(ItemData data , ShopType type) 
    {
        ImgIcon.sprite = data.icon;
        shopType = type;
        id = data.id;
        price = data.cost;
    }
    
    //public void ChooseItem()
    //{
    //    //Debug.Log(ITEM_ID + "======");   
    //    uiskinshop.ChooseItem(ITEM_ID, shopType);
    //}

    public void ChooseItem()
    {
        LevelManager.Ins.player.DestroyHat();
        //LevelManager.Ins.player.DestroyPant();

        LevelManager.Ins.player.InstantiateItem(id, shopType);
        
        uiskinshop.MONEY = price;
        uiskinshop.ITEM_ID = id;
        uiskinshop.SetPriceText(uiskinshop.MONEY);
    }

    public void CheckItem()
    {
        uiskinshop.CheckStatecOfChoosenItem(id, shopType);
    }
    
}
