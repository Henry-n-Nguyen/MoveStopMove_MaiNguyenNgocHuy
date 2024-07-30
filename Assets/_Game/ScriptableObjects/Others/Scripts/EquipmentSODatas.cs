using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "SciptableObjects/Others/EquipmentSODatas")]

[System.Serializable]
public class EquipmentSODatas : ScriptableObject
{
    public List<EquipmentSOData> listEquipmentSOData;
    public EquipmentSOData GetSOData(EquipmentType type)
    {
        foreach (EquipmentSOData item in listEquipmentSOData)
        {
            if (item.type == type) return item;
        }

        return null;
    }
}

[System.Serializable]
public class EquipmentSOData 
{
    public EquipmentType type;
    public List<EquipmentData> listEquipmentData = new List<EquipmentData>();

    public int Size => listEquipmentData.Count;

    public EquipmentData GetData(EquipmentId id)
    {
        foreach (EquipmentData item in listEquipmentData) 
        {
            if (item.id == id) return item;
        }

        return null;
    }

    public List<EquipmentData> GetList()
    {
        return listEquipmentData;
    }
}

[System.Serializable]
public class EquipmentData
{
    public Equipment prefab;
    public EquipmentId id;
    public int price;
    public Sprite sprite;
    public Material material;

    public T GetPrefab<T>() where T : Equipment
    {
        prefab.Init(id, price, sprite, material);
        return prefab as T;
    }
}
