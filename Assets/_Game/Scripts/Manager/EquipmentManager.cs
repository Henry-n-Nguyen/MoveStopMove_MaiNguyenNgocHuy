using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public List<Weapon> weaponList = new List<Weapon>();
    public List<Image> weaponImageList = new List<Image>();
    public List<Hat> hatList = new List<Hat>();

    private void Awake()
    {
        instance = this;
    }

    public Image GetImageById(int id)
    {
        Image weaponImage= weaponImageList[0];

        if (id < weaponImageList.Count) weaponImage = weaponImageList[id];

        return weaponImage;
    }

    public List<Image> GetWeaponImageList()
    {
        return weaponImageList;
    }

    public Weapon GetWeaponById(int id)
    {
        Weapon weaponWithId = weaponList[0];
        
        foreach (Weapon weapon in weaponList)
        {
            if (weapon.id == id) weaponWithId = weapon;
        }

        return weaponWithId;
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public Hat GetHatById(int id)
    {
        Hat hatWithId = hatList[0];

        foreach (Hat hat in hatList)
        {
            if (hat.id == id) hatWithId = hat;
        }

        return hatWithId;
    }

    public List<Hat> GetHatList()
    {
        return hatList;
    }

    public bool IsWeaponWithThisIdExist(int id)
    {
        foreach (Weapon weapon in weaponList)
        {
            if (weapon.id == id) return true;
        }

        return false;
    }
}
