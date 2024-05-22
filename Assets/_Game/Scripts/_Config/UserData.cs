using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public int coin;

    public List<int> weaponIdList = new List<int>();
    public List<int> hatIdList = new List<int>();
    public List<int> pantIdList = new List<int>();
    public List<int> skinIdList = new List<int>();
    public List<int> specialIdList = new List<int>();

    public bool isSpecialEquipped;
    public int equippedSpecialId = 0;

    public int equippedWeaponId;
    public int equippedHatId;
    public int equippedPantId;
    public int equippedSkinId;
}
