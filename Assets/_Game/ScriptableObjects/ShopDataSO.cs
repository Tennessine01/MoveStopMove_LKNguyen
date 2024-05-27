using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDataSO", menuName = "ScriptableObjects/ShopData", order = 1)]
[System.Serializable]
public class ShopDataSO : ScriptableObject
{
    [SerializeField] public List<ShopCategoryItem> shopCategories;
}

[Serializable]
public class ShopCategoryItem
{
    public ShopType shopType;
    public List<ItemData> listItems;
}

[System.Serializable]
public class ItemData
{
    public Sprite icon;
    public Material material;
    public int id;
    public int cost;
    public string description;
    public GameObject prefab;
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "SKinShopDataSO", menuName = "ScriptableObjects/ShopData", order = 1)]

//public class ShopDataSO : ScriptableObject
//{
//    public List<SkinShopItemData<HatType>> shopList;
//    //public SkinShopItemData<HatType> shopList;
//    //public SkinShopItemData<HatType> shopList;


//}


//[System.Serializable]
//public class SkinShopItemData<Enum> where Enum: System.Enum
//{
//    [SerializeField] public List<ItemData<Enum>> listItems;
//}

//// tuc la skinshop item data se chuyen enum vao itemdata sau do itemdata gan voi 1 item nao do.
//// sau do moi item se tuong ung voi 1 enum rieng de quan li
//[System.Serializable]
//public class ItemData<Enum>: ItemData where Enum : System.Enum
//{
//    public Enum type;
//}

//public class ItemData
//{
//    public Sprite icon;
//    public int ITEM_ID;
//    public int cost;
//    public string description;
//    public GameObject prefab;

//}