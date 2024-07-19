using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ItemState { Buy = 0, Bought = 1 , Equipped = 2, Selecting = 3 }

public class SkinShopItem : MonoBehaviour
{
    public GameObject lockIcon;
    public GameObject equippedIcon;
    public GameObject chooseIcon;
    public Image ImgIcon;
    public ItemState state;
    public SkinShop uiskinshop;
    public ShopType shopType;
    public int price;
    
    public int id;

    internal void Setup(ItemData data , ShopType type) 
    {
        ImgIcon.sprite = data.icon;
        shopType = type;
        id = data.id;
        price = data.cost;

        //check item is unlock or not
        if (UserDataManager.Ins.IsItemUnlocked(id, shopType))
        {
            UnlockItem();
        }

        //check if item is quipped
        if (UserDataManager.Ins.IsItemEquipped(id, shopType))
        {
            EquippedItem();
        }

        //// create unity action
        //onItemBoughtAction = (itemId, shopType) => OnItemBought(itemId, shopType);
        //onItemEquippedAction = (itemId, shopType) => OnItemEquipped(itemId, shopType);
        // subscribe to an event when bought

        uiskinshop.onItemBought.AddListener(OnItemBought);
        uiskinshop.onItemEquip.AddListener(OnItemEquipped);
    }

    public void ChooseItem()
    {
        LevelManager.Ins.player.DestroyHat();
        LevelManager.Ins.player.DestroyPant();

        LevelManager.Ins.player.InstantiateItem(id, shopType);

        uiskinshop.money = price;
        uiskinshop.itemID = id;
        uiskinshop.SetPriceText(uiskinshop.money);
        // close curent
        if (uiskinshop.currentSkinShopItem != null && uiskinshop.currentSkinShopItem != this)
        {
            uiskinshop.currentSkinShopItem.DeSelectItem();
        }

        chooseIcon.SetActive(true);
        uiskinshop.currentSkinShopItem = this;
        // open
    }
    private void DeSelectItem()
    {
        chooseIcon.SetActive(false);
    }
    public void CheckItem()
    {
        uiskinshop.CheckStateOfChosenItem(id, shopType);
    }

    public void UnlockItem()
    {
        lockIcon.SetActive(false);
    }
    public void EquippedItem()
    {
        equippedIcon.SetActive(true);
    }
    public void UnEquipItem()
    {
        equippedIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        if (uiskinshop != null)
        {
            uiskinshop.onItemBought.RemoveListener(OnItemBought);
            uiskinshop.onItemEquip.RemoveListener(OnItemEquipped);
        }
    }

    private void OnItemBought(int itemId, ShopType type)
    {
        if (id == itemId && shopType == type)
        {
            UnlockItem();
        }
    }

    private void OnItemEquipped(int itemId, ShopType type)
    {
        if (id == itemId && shopType == type)
        {
            EquippedItem();
        }
        else { UnEquipItem(); }
    }
}
