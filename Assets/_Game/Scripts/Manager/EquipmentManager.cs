using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [SerializeField] private List<Weapon> weaponList = new List<Weapon>();
    [SerializeField] private List<Sprite> weaponSpriteList = new List<Sprite>();
    [SerializeField] private List<Hat> hatList = new List<Hat>();
    [SerializeField] private List<Sprite> hatSpriteList = new List<Sprite>();

    private void Awake()
    {
        instance = this;
    }

    public Weapon GetWeaponById(int id)
    {
        foreach (Weapon weapon in weaponList)
        {
            if (weapon.id == id) return weapon;
        }

        return weaponList[id] != null ? weaponList[id] : weaponList[0];
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public List<Sprite> GetWeaponSpriteList()
    {
        return weaponSpriteList;
    }

    public Hat GetHatById(int id)
    {
        foreach (Hat hat in hatList)
        {
            if (hat.id == id) return hat;
        }

        return hatList[id] != null ? hatList[id] : hatList[0];
    }

    public List<Hat> GetHatList()
    {
        return hatList;
    }

    public List<Sprite> GetHatSpriteList()
    {
        return hatSpriteList;
    }
}
