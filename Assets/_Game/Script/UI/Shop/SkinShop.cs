using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum ShopType{
    Hat = 0, Pant = 1, Accessory = 2, Skin = 3, Weapon = 4
}
public enum EquipState{
    Buy = 0, Select = 1, Unequip = 2, NotBuy = 3
}
public class SkinShop : UICanvas
{
    [SerializeField] ShopDataSO skinShopData; // data cua cac item lay tu SO 
    [SerializeField] SkinShopItem skinShopItemPrefab; // prefab item 
    [SerializeField] Transform container; // vi tri chua cac item sau khi sinh ra ( trong UI SHOP )

    [SerializeField] TextMeshProUGUI playerCoinTxt; // so tien cua User
    public List<SkinShopItem> skinShopItemsList; // list chua cac item sau khi da truyen data

    [SerializeField] List<GameObject> buyState; // list cac button Buy,Select,Unequip
    private ShopType currentShopType; // trang thai shop hien tai

    [SerializeField] private  TMP_Text[] priceText; // so tien  cua item

    [HideInInspector] public int MONEY = 0; // bien chua gia cua item dang chon
    [HideInInspector] public int ITEM_ID = 0; // bien chua id cua item dang chon

    public UnityEvent<int, ShopType> onItemBought = new UnityEvent<int, ShopType>();
    public UnityEvent<int, ShopType> onItemEquip = new UnityEvent<int, ShopType>();



    public override void Open()
    {
        base.Open();
        LevelManager.Ins.SetCameraShop();

        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());

        // mac dinh mo muc hat
        ChangeShopType(ShopType.Hat);
        //priceText.text = "";
    }
    private List<ItemData> GetListItemsByShopType(ShopType shopType)
    {
        List<ShopCategoryItem> listShopCategories = skinShopData.shopCategories;
        foreach ( ShopCategoryItem category in listShopCategories)
        {
            if (category.shopType == shopType)
            {
                return category.listItems;
            }
        }
        return null;
    }
    //-----------------nut tren shop bar dung de chuyen trang thai shoptype---------------
    public void ShopBarButton(int shopType)
    {
        ChangeShopType((ShopType)shopType);
    }
    //---------------- tu trang thai shoptype lay ra cac list item tuong ung trong SO -------
    public void ChangeShopType(ShopType shopType)
    {
        currentShopType = shopType;

        // xoa cac item hien tai
        DestroySkinShopItems();

        // lay danh sach item dua tren loai shop
        List<ItemData> items = GetListItemsByShopType(shopType);

        // tao cac item moi
        foreach (ItemData item in items)
        {
            SkinShopItem skinShopItem = Instantiate(skinShopItemPrefab, container);
            /// ------- tai sao prefab lai bi thay doi --------------------------------------------------------------------
            skinShopItem.uiskinshop = this;
            skinShopItem.Setup(item, shopType);
            skinShopItemsList.Add(skinShopItem);
            //priceText.SetText(item.ToString());

        }
    }
    public void SetPriceText(int price)
    {
        priceText[0].text = price.ToString();
        priceText[1].text = price.ToString();
    }
    //------------ de chuyen trang thai mua, select hay unequip cho item ----------
    public void CheckStatecOfChoosenItem(int id, ShopType type)
    {
        switch (type){
            case(ShopType.Hat):
                if (UserDataManager.Ins.userData.hatList.Contains(id)){
                    if (UserDataManager.Ins.userData.currentHat == id){
                        ChangeButtonBuyState(EquipState.Unequip);
                        break;
                    }
                    else{
                        ChangeButtonBuyState(EquipState.Select);
                        break;
                    }
                }
                if(UserDataManager.Ins.userData.coin < MONEY)
                {
                    ChangeButtonBuyState (EquipState.NotBuy);
                }
                else ChangeButtonBuyState(EquipState.Buy);
                break;
            case (ShopType.Pant):
                if (UserDataManager.Ins.userData.pantList.Contains(id)){
                    if (UserDataManager.Ins.userData.currentPant == id){
                        ChangeButtonBuyState(EquipState.Unequip);
                        break;
                    }
                    else{
                        ChangeButtonBuyState(EquipState.Select);
                        break;
                    }
                }
                if (UserDataManager.Ins.userData.coin < MONEY)
                {
                    ChangeButtonBuyState(EquipState.NotBuy);
                }
                else ChangeButtonBuyState(EquipState.Buy);
                break;
            case (ShopType.Accessory):
                if (UserDataManager.Ins.userData.accessoryList.Contains(id))
                {
                    if (UserDataManager.Ins.userData.currentSkin == id)
                    {
                        ChangeButtonBuyState(EquipState.Unequip);
                        break;
                    }
                    else
                    {
                        ChangeButtonBuyState(EquipState.Select);
                        break;
                    }
                }
                if (UserDataManager.Ins.userData.coin < MONEY)
                {
                    ChangeButtonBuyState(EquipState.NotBuy);
                }
                else ChangeButtonBuyState(EquipState.Buy);
                break;

        }
    }
    //------------------ tu trang thai da chon, bat tat cac button tuong ung -----------
    public void ChangeButtonBuyState(EquipState state)
    {
        switch(state)
        {
            case (EquipState.Buy):
                SetActiveBuyState(0);
                break;
            case (EquipState.Select):
                SetActiveBuyState(1);
                break;
            case (EquipState.Unequip):
                SetActiveBuyState(2);
                break;
            case (EquipState.NotBuy): 
                SetActiveBuyState(3);
                break;
        }
    }
    //--------------------------- cac button de lua chon -----------------------------------------------
    public void BuyItem()
    {
        if(UserDataManager.Ins.userData.coin >= MONEY)
        {
            UserDataManager.Ins.userData.coin -= MONEY;
            playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());

            SetActiveBuyState(2);
            if (currentShopType == ShopType.Hat)
            {
                UserDataManager.Ins.userData.hatList.Add(ITEM_ID);
                UserDataManager.Ins.userData.currentHat = ITEM_ID;
            }
            if (currentShopType == ShopType.Pant)
            {
                UserDataManager.Ins.userData.pantList.Add(ITEM_ID);
                UserDataManager.Ins.userData.currentPant = ITEM_ID;
            }
            if (currentShopType == ShopType.Accessory)
            {
                UserDataManager.Ins.userData.accessoryList.Add(ITEM_ID);
                UserDataManager.Ins.userData.currentAccessory = ITEM_ID;
            }
            //de bat tat icon equiped hoac lock
            onItemBought?.Invoke(ITEM_ID, currentShopType);
            onItemEquip?.Invoke(ITEM_ID, currentShopType);

        }
    }
    public void CantNotBuy()
    {
        SetActiveBuyState(3);
    }
    public void UnequipItem()
    {
        SetActiveBuyState(1);
        
        if (currentShopType == ShopType.Hat)
        {
            LevelManager.Ins.player.DestroyHat();
            UserDataManager.Ins.userData.currentHat = 0;
        }
        if (currentShopType == ShopType.Pant)
        {
            LevelManager.Ins.player.DestroyPant();
            UserDataManager.Ins.userData.currentPant = 0;
        }
        if (currentShopType == ShopType.Accessory)
        {
            LevelManager.Ins.player.DeActiveShield();
            UserDataManager.Ins.userData.currentAccessory = 0;
        }
        onItemEquip?.Invoke(1000, currentShopType);

    }
    public void SelectItem()
    {
        SetActiveBuyState(2);
        if (currentShopType == ShopType.Hat)
        {
            LevelManager.Ins.player.DestroyHat();
            LevelManager.Ins.player.InstantiateHat(ITEM_ID);
            UserDataManager.Ins.userData.currentHat = ITEM_ID;
        }
        if (currentShopType == ShopType.Pant)
        {
            LevelManager.Ins.player.DestroyPant();
            LevelManager.Ins.player.InstantiatePant(ITEM_ID);
            UserDataManager.Ins.userData.currentPant = ITEM_ID;
        }
        if (currentShopType == ShopType.Accessory)
        {
            LevelManager.Ins.player.DeActiveShield();
            LevelManager.Ins.player.ActiveShield(ITEM_ID);
            UserDataManager.Ins.userData.currentAccessory = ITEM_ID;
        }
        onItemEquip?.Invoke(ITEM_ID, currentShopType);
    }
    //chuyen trang thai cac button buy, select, unequip
    public void SetActiveBuyState(int index)
    {
        for (int i = 0; i < buyState.Count; i++)
        {
            if (i == index)
            {
                buyState[i].SetActive(true);
            }
            else
            {
                buyState[i].SetActive(false);
            }
        }
    }
    //------------- thoat ui ----------------------

    public void BackButton()
    {
        Close(0);
        DestroySkinShopItems();
        LevelManager.Ins.player.DestroyHat();
        LevelManager.Ins.player.DestroyPant();
        LevelManager.Ins.player.DestroyWeapon();
        LevelManager.Ins.player.DeActiveShield();

        GameManager.Ins.ChangeState(GameState.MainMenu);
        //LevelManager.Ins.player.OnInit();
    }
    private void DestroySkinShopItems()
    {
        foreach (var item in skinShopItemsList)
        {
            Destroy(item.gameObject);
        }
        skinShopItemsList.Clear();
    }
}

