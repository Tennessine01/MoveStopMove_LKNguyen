using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private const string KEY_USER_DATA = "UserData";

    public UserData userData;
    [SerializeField] Player player;

    private void OnApplicationQuit(){Save();}

    private void OnApplicationPause(bool pauseStatus) { if (pauseStatus) { Save(); } }
    
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(KEY_USER_DATA, ""))) {
            userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(KEY_USER_DATA, ""));
        }
        else
        {
            userData = new();
        }
    
    }

    public void Save()
    {
        PlayerPrefs.SetString(KEY_USER_DATA, JsonUtility.ToJson(userData));
    }

    //kiem tra item da unlock hay chua
    public bool IsItemUnlocked(int itemId, ShopType shopType)
    {
        switch (shopType)
        {
            case ShopType.Hat:
                return userData.hatList.Contains(itemId);
            case ShopType.Pant:
                return userData.pantList.Contains(itemId);
            case ShopType.Accessory:
                return userData.accessoryList.Contains(itemId);
            default:
                return false;
        }
    }

    public bool IsItemEquipped(int itemId, ShopType shopType)
    {
        switch (shopType)
        {
            case ShopType.Hat:
                return userData.currentHat == itemId;
            case ShopType.Pant:
                return userData.currentPant == itemId;
            case ShopType.Accessory:
                return userData.currentAccessory == itemId;
            default:
                return false;
        }
    }

}
