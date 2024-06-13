using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "EquipmentDataSO")]
public class EquipmentDataSO : ScriptableObject
{
    private Dictionary<EquipmentType, List<Sprite>> equipmentSpriteList = new Dictionary<EquipmentType, List<Sprite>>();

    [SerializeField] private List<Weapon> weaponList;
    [SerializeField] private List<Sprite> weaponSpriteList;

    [SerializeField] private List<Hat> hatList;
    [SerializeField] private List<Sprite> hatSpriteList;

    [SerializeField] private List<Skin> skinList;
    [SerializeField] private List<Sprite> skinSpriteList;

    [SerializeField] private List<Pant> pantList;
    [SerializeField] private List<Sprite> pantSpriteList;

    [SerializeField] private List<Special> specialList;
    [SerializeField] private List<Sprite> specialSpriteList;

    public List<Sprite> GetEquipmentSpriteListByType(EquipmentType type)
    {
        if (!equipmentSpriteList.ContainsKey(type))
        {
            switch (type)
            {
                case EquipmentType.Weapon: equipmentSpriteList.Add(type, weaponSpriteList); break;
                case EquipmentType.Hat: equipmentSpriteList.Add(type, hatSpriteList); break;
                case EquipmentType.Skin: equipmentSpriteList.Add(type, skinSpriteList); break;
                case EquipmentType.Pant: equipmentSpriteList.Add(type, pantSpriteList); break;
                case EquipmentType.Special: equipmentSpriteList.Add(type, specialSpriteList); break;
            }
        }

        return equipmentSpriteList[type];
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
