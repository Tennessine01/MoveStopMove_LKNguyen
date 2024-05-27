
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipemtManager : Singleton<EquipemtManager>
{
    [SerializeField]
    private ShopDataSO shopData;
    [SerializeField]
    public Material defaultPantMaterial;

    public GameObject InstantiatePrefabById(int id, Transform position, ShopType shoptype)
    {
        GameObject prefab = GetPrefabById(id, shoptype );
        if (prefab != null)
        {
            return Instantiate(prefab, position);
        }
        return null;
    }

    private GameObject GetPrefabById(int id, ShopType shopType)
    {
        foreach (var category in shopData.shopCategories)
        {
            if (category.shopType == shopType)
            {
                var listItem = category.listItems;
                for (int j = 0; j < listItem.Count; j++)
                {
                    var item = listItem[j];
                    if (item.id == id)
                    {
                        return item.prefab;
                    }
                }
            }
        }
        return null;
    }
    

    public Material GetMaterialByID(int id, ShopType shopType)
    {
        foreach (var category in shopData.shopCategories)
        {
            if (category.shopType == shopType)
            {
                var listItem = category.listItems;
                for (int j = 0; j < listItem.Count; j++)
                {
                    var item = listItem[j];
                    if (item.id == id)
                    {
                        return item.material;
                    }
                }
            }
        }
        return null;
    }
}












//using System.Collections.Generic;
//using UnityEngine;

//public class EquipemtManager : Singleton<EquipemtManager> 
//{
//    [System.Serializable]
//    public struct PrefabEntry
//    {
//        public int ITEM_ID;
//        public GameObject material;
//    }
//    [System.Serializable] 
//    public struct MaterialEntry
//    {
//        public int ITEM_ID;
//        public Material material;
//    }

//    [SerializeField]
//    private List<PrefabEntry> prefabEntries;
//    [Space(20)]
//    [SerializeField]
//    private List<MaterialEntry> materialEntries;

//    private Dictionary<int, GameObject> prefabDictionary;
//    private Dictionary<int, Material> materialDictionary;


//    public void Start()
//    {
//        prefabDictionary = new Dictionary<int, GameObject>();
//        foreach (PrefabEntry entry in prefabEntries)
//        {
//            if (!prefabDictionary.ContainsKey(entry.ITEM_ID))
//            {
//                prefabDictionary.Add(entry.ITEM_ID, entry.material);
//            }
//        }

//        materialDictionary = new Dictionary<int, Material>();
//        foreach (MaterialEntry entry in materialEntries)
//        {
//            if (!materialDictionary.ContainsKey(entry.ITEM_ID))
//            {
//                materialDictionary.Add(entry.ITEM_ID, entry.material);
//            }
//        }
//    }

//    public GameObject GetPrefabById(int ITEM_ID)
//    {
//        if (prefabDictionary.TryGetValue(ITEM_ID, out GameObject material))
//        {
//            return material;
//        }
//        else
//        {
//            return null;
//        }
//    }
//    public Material SetMaterialById(int ITEM_ID)
//    {
//        if (materialDictionary.TryGetValue(ITEM_ID, out Material material))
//        {
//            return material;
//        }
//        else
//        {
//            return null;
//        }
//    }

//    public GameObject InstantiatePrefabById(int ITEM_ID, Transform position)
//    {
//        GameObject material = GetPrefabById(ITEM_ID);
//        if (material != null)
//        {
//            return Instantiate(material,position);
//        }
//        return null;
//    }
//}

