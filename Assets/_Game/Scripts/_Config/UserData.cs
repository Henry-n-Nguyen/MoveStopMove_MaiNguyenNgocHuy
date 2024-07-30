using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

[Serializable]
public class UserData
{
    // Public
    public List<EquipmentId> equipmentBought = new List<EquipmentId>();

    //public List<WeaponId> WeaponList = new List<WeaponId>();
    //public List<HatId> HatList = new List<HatId>();
    //public List<SkinId> SkinList = new List<SkinId>();
    //public List<PantId> PantList = new List<PantId>();
    //public List<AccessoryId> AccessoryList = new List<AccessoryId>();

    // Coin
    public int coin;

    //Setting
    public bool isVibrationOn = true;
    public bool isSoundOn = true;

    //Config
    public int currentLevel = 0;

    public int currentHighestLevel = 0;
    public int currentHighestRank = 50;

    //Equipment
    public List<EquipmentId> equippedEquipment = new List<EquipmentId>();
}
