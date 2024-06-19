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
    public string name;
    public string description;
    public GameObject prefab;
}
