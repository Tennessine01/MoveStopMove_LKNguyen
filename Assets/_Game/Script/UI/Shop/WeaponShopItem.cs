using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponShopItem : MonoBehaviour
{
    public Image ImgIcon;
    public ItemState state;
    public UIWeaponShop uiWeaponShop;
    //public ShopType shopType;
    public int price;

    public int id;
    public TMP_Text weaponName;
    public void Setup(ItemData data)
    {
        ImgIcon.sprite = data.icon;
        //shopType = type;
        id = data.id;
        price = data.cost;
        weaponName.text = data.name.ToString();
    }

    
}
