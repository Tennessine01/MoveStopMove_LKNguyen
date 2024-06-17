using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponShop : UICanvas
{
    [SerializeField] TextMeshProUGUI playerCoinTxt; // so tien cua User

    [SerializeField] ShopDataSO weaponShopData; // data cua cac item lay tu SO 
    [SerializeField] WeaponShopItem weaponShopItemPrefab; // prefab item 
    [SerializeField] Transform container; // vi tri chua cac item sau khi sinh ra ( trong UI SHOP )
    [SerializeField] private TMP_Text[] priceText; // so tien  cua item

    [SerializeField] List<GameObject> buyState; // list cac button Buy,Select,Unequip

    public List<WeaponShopItem> weaponShopItemsList; // list chua cac item sau khi da truyen data

    private int MONEY = 0;
    private int ITEM_ID = 0;

    
    [SerializeField] private int currentItemIndex = 0;

    public override void Open()
    {
        base.Open();
        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());
        LoadWeaponData();
        
    }

    public void LoadWeaponData()
    {

        // xoa cac item hien tai
        DestroyWeaponShopItems();

        // lay danh sach item dua tren loai shop
        List<ItemData> items = GetListWeaponItems();

        // tao cac item moi
        for (int i = 1; i < items.Count; i++)
        {
            ItemData item = items[i];
            WeaponShopItem weaponShopItem = Instantiate(weaponShopItemPrefab, container);
            weaponShopItem.gameObject.SetActive(false);
            //weaponShopItem.uiWeaponShop = this;
            weaponShopItem.Setup(item);
            weaponShopItemsList.Add(weaponShopItem);
            //priceText.SetText(item.ToString());
        }

        if (weaponShopItemsList.Count > 0)
        {
            currentItemIndex = 1;
            weaponShopItemsList[currentItemIndex].gameObject.SetActive(true);
            CheckStateOfItem(weaponShopItemsList[currentItemIndex].id);
            SetPriceText(weaponShopItemsList[currentItemIndex].price);

        }
    }
    public void ShowPreviousItem()
    {
        if (weaponShopItemsList.Count == 0)
        {
            return;
        }
        weaponShopItemsList[currentItemIndex].gameObject.SetActive(false);

        currentItemIndex--;
        if (currentItemIndex < 0)
        {
            currentItemIndex = weaponShopItemsList.Count - 1;
        }
        weaponShopItemsList[currentItemIndex].gameObject.SetActive(true);
        CheckStateOfItem(weaponShopItemsList[currentItemIndex].id);
        SetPriceText(weaponShopItemsList[currentItemIndex].price);

    }

    public void ShowNextItem()
    {
        if (weaponShopItemsList.Count == 0)
        {
            return;
        }

        weaponShopItemsList[currentItemIndex].gameObject.SetActive(false);

        currentItemIndex++;
        if (currentItemIndex > weaponShopItemsList.Count -1)
        {
            currentItemIndex = 0;
        }
        weaponShopItemsList[currentItemIndex].gameObject.SetActive(true);
        CheckStateOfItem(weaponShopItemsList[currentItemIndex].id);
        SetPriceText(weaponShopItemsList[currentItemIndex].price);
    }


    private List<ItemData> GetListWeaponItems()
    {
        for (int i = 0; i < weaponShopData.shopCategories.Count; i++)
        {
            ShopCategoryItem category = weaponShopData.shopCategories[i];
            if (category.shopType == ShopType.Weapon)
            {
                return category.listItems;
            }
        }
        return null;
    }
    //---------------
    public void CheckStateOfItem(int id)
    {

        if (UserDataManager.Ins.userData.weaponList.Contains(id))
        {
            if (UserDataManager.Ins.userData.currentWeapon == id)
            {
                ChangeButtonBuyState(EquipState.Unequip);
            }
            else
            {
                ChangeButtonBuyState(EquipState.Select);
            }
        }
        if (UserDataManager.Ins.userData.coin < weaponShopItemsList[currentItemIndex].price)
        {
            ChangeButtonBuyState(EquipState.NotBuy);
        }
        else ChangeButtonBuyState(EquipState.Buy);
    }

    //------------------ tu trang thai da chon, bat tat cac button tuong ung -----------
    public void ChangeButtonBuyState(EquipState state)
    {
        switch (state)
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

    //-----------------------------------------------------------------------------
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
    //------------------------------------------------------
    public void BuyItem()
    {
        if (UserDataManager.Ins.userData.coin >= weaponShopItemsList[currentItemIndex].price)
        {
            UserDataManager.Ins.userData.coin -= weaponShopItemsList[currentItemIndex].price;
            playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());

            SetActiveBuyState(2);
            
            
            UserDataManager.Ins.userData.weaponList.Add(weaponShopItemsList[currentItemIndex].id);
            UserDataManager.Ins.userData.currentWeapon = weaponShopItemsList[currentItemIndex].id;
        }
    }
    public void UnequipItem()
    {
        SetActiveBuyState(1);
        LevelManager.Ins.player.DestroyWeapon();

        UserDataManager.Ins.userData.currentWeapon = 0;
    }
    public void SelectItem()
    {
        SetActiveBuyState(2);
        LevelManager.Ins.player.DestroyWeapon();
        LevelManager.Ins.player.InstantiateWeapon(weaponShopItemsList[currentItemIndex].id);
        UserDataManager.Ins.userData.currentWeapon = weaponShopItemsList[currentItemIndex].id;
    }
    public void CantNotBuy()
    {
        SetActiveBuyState(3);
    }
    //--------------------------------------------
    private void DestroyWeaponShopItems()
    {
        foreach (var item in weaponShopItemsList)
        {
            Destroy(item.gameObject);
        }
        weaponShopItemsList.Clear();
    }
    

    public void SetPriceText(int price)
    {
        priceText[0].text = price.ToString();
        priceText[1].text = price.ToString();
    }
    //-------------thoat ui----------------------------------------
    public void BackButton()
    {
        Close(0);
        DestroyWeaponShopItems();
        LevelManager.Ins.player.DestroyHat();
        LevelManager.Ins.player.DestroyPant();
        LevelManager.Ins.player.DestroyWeapon();
        //LevelManager.Ins.player.ResetItem();

        //LevelManager.Ins.ActivatePlayer();
        //LevelManager.Ins.player.OnInit();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        //UIManager.Ins.OpenUI<UIMainMenu>();
        //LevelManager.Ins.SpawnPlayer();

    }
}
