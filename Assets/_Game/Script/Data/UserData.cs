using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    ////Range
    public float range;
    //Movespeed
    public float moveSpeed;
    ////AttackSpeed
    public float attackSpeed;
    ////Size
    public float size;
    //Skin

    public int coin;
    //ItemData
    public List<int> weaponList = new();
    public List<int> hatList = new();
    public List<int> pantList = new();
    public int currentWeapon;
    public int currentHat;
    public int currentPant;

}
