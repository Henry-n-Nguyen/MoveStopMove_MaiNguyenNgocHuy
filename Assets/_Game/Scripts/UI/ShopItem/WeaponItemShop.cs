using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class WeaponItemShop : ItemShop
{
    [Header("Unique Item References")]
    [SerializeField] protected GameObject lockUI;

    public void Init(Sprite sprite, EquipmentType type, EquipmentId id, int price)
    {
        this.icon.sprite = sprite;

        this.Type = type;
        this.Id = id;
        this.Price = price;
    }

    public void OnLock()
    {
        lockUI.SetActive(true);
    }

    public void OnUnlock()
    {
        lockUI.SetActive(false);
    }
}
