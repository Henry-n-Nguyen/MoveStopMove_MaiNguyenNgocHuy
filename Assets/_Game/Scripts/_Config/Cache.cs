using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Cache
{
    private static Dictionary<Collider, AbstractCharacter> characters = new Dictionary<Collider, AbstractCharacter>();

    public static AbstractCharacter GetCharacter(Collider collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<AbstractCharacter>());
        }

        return characters[collider];
    }

    private static Dictionary<Collider, Transform> spawnPoints = new Dictionary<Collider, Transform>();

    public static Transform GetSpawnpoint(Collider collider)
    {
        if (!spawnPoints.ContainsKey(collider))
        {
            spawnPoints.Add(collider, collider.transform);
        }

        return spawnPoints[collider];
    }

    private static Dictionary<EquipmentType, List<ItemShop>> itemLists = new Dictionary<EquipmentType, List<ItemShop>>();

    public static List<ItemShop> GetItemList(EquipmentType equipmentType)
    {
        if (!itemLists.ContainsKey(equipmentType))
        {
            List<ItemShop> list = new List<ItemShop>();
            itemLists.Add(equipmentType, list);
        }

        return itemLists[equipmentType];
    }
}
