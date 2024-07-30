using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeItemShop : ItemShop
{
    [Header("Unique Item References")]
    [SerializeField] protected GameObject buttonBorder;

    private CostumeShop shopScript;

    public void Init(Sprite sprite, EquipmentType type, EquipmentId id, int price, CostumeShop shopScript)
    {
        this.icon.sprite = sprite;

        this.Type = type;
        this.Id = id;
        this.Price = price;

        this.shopScript = shopScript;
    }

    public override void OnInit()
    {
        base.OnInit();

        button.onClick.AddListener(OnImageClick);
    }

    public void OnImageClick()
    {
        shopScript.OnItemClick(this);
    }

    public void OnClick()
    {
        buttonBorder.SetActive(true);
    }

    public void OnRelease()
    {
        buttonBorder.SetActive(false);
    }
}
