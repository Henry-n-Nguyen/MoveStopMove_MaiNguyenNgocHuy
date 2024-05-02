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
        try
        {
            return weaponList[id];
        }
        catch
        {
            return weaponList[0];
        }
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public Hat GetHatById(int id)
    {
        try
        {
            return hatList[id];
        }
        catch
        {
            return hatList[0];
        }
    }

    public List<Hat> GetHatList()
    {
        return hatList;
    }
}
