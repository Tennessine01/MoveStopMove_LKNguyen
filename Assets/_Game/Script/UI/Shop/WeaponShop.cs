using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShop : UICanvas
{
    [SerializeField] TextMeshProUGUI playerCoinTxt; // so tien cua User

    [SerializeField] ShopDataSO weaponShopData; // data cua cac item lay tu SO 
    [SerializeField] WeaponShopItem weaponShopItemPrefab; // prefab item 
    [SerializeField] Transform container; // vi tri chua cac item sau khi sinh ra ( trong UI SHOP )
    [SerializeField] private TMP_Text[] priceText; // so tien  cua item

    [SerializeField] List<GameObject> buyState; // list cac button Buy,Select,Unequip

    public List<WeaponShopItem> weaponShopItemsList = new List<WeaponShopItem>(); // list chua cac item sau khi da truyen data

    [SerializeField] private int currentItemIndex = 0;
    private bool isInitialized = false;
    public override void Open()
    {
        base.Open();
        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());

        if (!isInitialized)
        {
            InitializeShop();
            isInitialized = true;
        }
        else
        {
            ActivateWeaponShopItems();
        }

        if (weaponShopItemsList.Count > 0)
        {
            currentItemIndex = 0;
            ShowCurrentItem();
        }
    }

    private void InitializeShop()
    {
        List<ItemData> items = GetListWeaponItems();

        for (int i = 1; i < items.Count; i++)
        {
            ItemData item = items[i];
            WeaponShopItem weaponShopItem = Instantiate(weaponShopItemPrefab, container);
            weaponShopItem.gameObject.SetActive(false);
            weaponShopItem.Setup(item);
            weaponShopItemsList.Add(weaponShopItem);
        }
    }
    private void ActivateWeaponShopItems()
    {
        foreach (WeaponShopItem item in weaponShopItemsList)
        {
            item.gameObject.SetActive(false);
        }
    }

    //public void LoadWeaponData()
    //{

    //    // xoa cac item hien tai
    //    DestroyWeaponShopItems();

    //    // lay danh sach item dua tren loai shop
    //    List<ItemData> items = GetListWeaponItems();

    //    // tao cac item moi
    //    for (int i = 1; i < items.Count; i++)
    //    {
    //        ItemData item = items[i];
    //        WeaponShopItem weaponShopItem = Instantiate(weaponShopItemPrefab, container);
    //        weaponShopItem.gameObject.SetActive(false);
    //        //weaponShopItem.uiWeaponShop = this;
    //        weaponShopItem.Setup(item);
    //        weaponShopItemsList.Add(weaponShopItem);
    //        //priceText.SetText(item.ToString());
    //    }

    //    if (weaponShopItemsList.Count > 0)
    //    {
    //        currentItemIndex = 1;
    //        weaponShopItemsList[currentItemIndex].gameObject.SetActive(true);
    //        CheckStateOfItem(weaponShopItemsList[currentItemIndex].id);
    //        SetPriceText(weaponShopItemsList[currentItemIndex].price);

    //    }
    //}
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
        ShowCurrentItem();
    }

    public void ShowNextItem()
    {
        if (weaponShopItemsList.Count == 0)
        {
            return;
        }

        weaponShopItemsList[currentItemIndex].gameObject.SetActive(false);
        currentItemIndex++;

        if (currentItemIndex >= weaponShopItemsList.Count)
        {
            currentItemIndex = 0;
        }
        ShowCurrentItem();
    }
    private void ShowCurrentItem()
    {
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
    //--------------------------------------------------------------
    public void CheckStateOfItem(int id)
    {

        if (UserDataManager.Ins.userData.weaponList.Contains(id))
        {
            if (UserDataManager.Ins.userData.currentWeapon == id)
            {
                ChangeButtonBuyState(EquipState.Unequip);
                return;
            }
            else
            {
                ChangeButtonBuyState(EquipState.Select);
                return;
            }
        }
        else if (UserDataManager.Ins.userData.coin < weaponShopItemsList[currentItemIndex].price)
        {
            ChangeButtonBuyState(EquipState.CanNotBuy);
        }
        else ChangeButtonBuyState(EquipState.Buy);
    }

    //------------------ tu trang thai da chon, bat tat cac button tuong ung -----------
    private void ChangeButtonBuyState(EquipState state)
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
            case (EquipState.CanNotBuy):
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
        foreach (WeaponShopItem item in weaponShopItemsList)
        {
            //Destroy(item.gameObject);
            item.gameObject.SetActive(false);

        }
        //weaponShopItemsList.Clear();
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
        GameManager.Ins.ChangeState(GameState.MainMenu);
        

    }
}
