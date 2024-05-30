using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ItemState { Buy = 0, Bought = 1 , Equipped = 2, Selecting = 3 }


public class SkinShopItem : MonoBehaviour
{
    [SerializeField] public GameObject lockIcon;
    [SerializeField] public GameObject equippedIcon;
    public Image ImgIcon;
    public ItemState state;
    public UISkinShop uiskinshop;
    public ShopType shopType;
    public int price;
    
    public int id;

    private UnityAction<int, ShopType> onItemBoughtAction;
    private UnityAction<int, ShopType> onItemEquippedAction;

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

        // create unity action
        onItemBoughtAction = (itemId, shopType) => OnItemBought(itemId, shopType);
        onItemEquippedAction = (itemId, shopType) => OnItemEquipped(itemId, shopType);
        // subscribe to an event when bought
        uiskinshop.onItemBought.AddListener(onItemBoughtAction);
        uiskinshop.onItemEquip.AddListener(onItemEquippedAction);
    }

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
            uiskinshop.onItemBought.RemoveListener(onItemBoughtAction);
            uiskinshop.onItemEquip.RemoveListener(onItemEquippedAction);
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
