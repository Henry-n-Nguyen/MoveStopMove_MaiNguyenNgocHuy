using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public List<Weapon> weaponList = new List<Weapon>();
    public List<Hat> hatList = new List<Hat>();

    private void Awake()
    {
        instance = this;
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
}
