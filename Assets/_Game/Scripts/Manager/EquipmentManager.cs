using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Dictionary<EquipmentType, List<Sprite>> equipmentSpriteList = new Dictionary<EquipmentType, List<Sprite>>();

    [SerializeField] private List<Weapon> weaponList = new List<Weapon>();
    [SerializeField] private List<Sprite> weaponSpriteList = new List<Sprite>();

    [SerializeField] private List<Hat> hatList = new List<Hat>();
    [SerializeField] private List<Sprite> hatSpriteList = new List<Sprite>();

    [SerializeField] private List<Skin> skinList = new List<Skin>();
    [SerializeField] private List<Sprite> skinSpriteList = new List<Sprite>();

    [SerializeField] private List<Pant> pantList = new List<Pant>();
    [SerializeField] private List<Sprite> pantSpriteList = new List<Sprite>();

    [SerializeField] private List<Special> specialList = new List<Special>();
    [SerializeField] private List<Sprite> specialSpriteList = new List<Sprite>();

    private void Awake()
    {
        instance = this;

        equipmentSpriteList[EquipmentType.Weapon] = weaponSpriteList;
        equipmentSpriteList[EquipmentType.Hat] = hatSpriteList;
        equipmentSpriteList[EquipmentType.Skin] = skinSpriteList;
        equipmentSpriteList[EquipmentType.Pant] = pantSpriteList;
        equipmentSpriteList[EquipmentType.Special] = specialSpriteList;
    }

    // Weapon
    public Weapon GetWeaponById(int id)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i].id == id) return weaponList[i];
        }

        return weaponList[id] != null ? weaponList[id] : weaponList[0];
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    // Hat
    public Hat GetHatById(int id)
    {
        for (int i = 0; i < hatList.Count; i++)
        {
            if (hatList[i].id == id) return hatList[i];
        }

        return hatList[id] != null ? hatList[id] : hatList[0];
    }

    public List<Hat> GetHatList()
    {
        return hatList;
    }

    // Skin
    public Skin GetSkinById(int id)
    {
        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].id == id) return skinList[i];
        }

        return skinList[id] != null ? skinList[id] : skinList[0];
    }

    public List<Skin> GetSkinList()
    {
        return skinList;
    }

    // Pant
    public Pant GetPantById(int id)
    {
        for (int i = 0; i < pantList.Count; i++)
        {
            if (pantList[i].id == id) return pantList[i];
        }

        return pantList[id] != null ? pantList[id] : pantList[0];
    }

    public List<Pant> GetPantList()
    {
        return pantList;
    }

    // Special
    public Special GetSpecialById(int id)
    {
        id++;

        for (int i = 0; i < specialList.Count; i++)
        {
            if (specialList[i].id == id) return specialList[i];
        }

        return specialList[id] != null ? specialList[id] : specialList[0];
    }

    public List<Special> GetSpecialList()
    {
        return specialList;
    }
}
