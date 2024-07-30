using HuySpace;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    [Header("EquipmentDataSO")]
    [SerializeField] private EquipmentSODatas equipmentSODatas;

    [Header("Prefab")]
    [SerializeField] private WeaponItemShop weaponPrefab;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RectTransform content;

    [Header("Buttons")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject equippedButton;
    [SerializeField] private GameObject lockedButton;

    [Header("Notification")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationHolder;


    // In Editor
    // Current shop item Clicked
    private EquipmentType currentTypeOfShop = EquipmentType.Weapon;
    private WeaponItemShop currentItemOnShow = null;
    private UserData data;

    private int index;

    private List<WeaponItemShop> items = new List<WeaponItemShop>();

    // Function
    private void Awake()
    {
        data = UserDataManager.Ins.userData;

        InitItemShop(currentTypeOfShop);
    }

    public override void Setup()
    {
        base.Setup();

        DisplayWeapon(0);
        CheckButtonState();

        coinText.text = data.coin.ToString();
    }

    private void CheckButtonState()
    {
        EquipmentType type = currentItemOnShow.Type;
        EquipmentId id = currentItemOnShow.Id;
        int price = currentItemOnShow.Price;

        if (!data.equipmentBought.Contains(id))
        {
            if (!data.equipmentBought.Contains(items[index - 1].Id))
            {
                currentItemOnShow.OnLock();
                ChangeButton(ButtonType.LockedButton);
            }
            else
            {
                currentItemOnShow.OnUnlock();
                ChangeButton(ButtonType.BuyButton);
                priceText.text = price.ToString();
            }
        }
        else
        {
            if (id == data.equippedEquipment[(int)EquipmentType.Weapon])
            {
                ChangeButton(ButtonType.EquippedButton);
            }
            else
            {
                ChangeButton(ButtonType.EquipButton);
            }
        }
    }

    private void ChangeButton(ButtonType type)
    {
        buyButton.SetActive(false);
        equipButton.SetActive(false);
        equippedButton.SetActive(false);
        lockedButton.SetActive(false);

        switch (type)
        {
            case ButtonType.BuyButton:
                buyButton.SetActive(true);
                break;
            case ButtonType.EquipButton:
                equipButton.SetActive(true);
                break;
            case ButtonType.EquippedButton:
                equippedButton.SetActive(true);
                break;
            case ButtonType.LockedButton:
                lockedButton.SetActive(true);
                break;
        }
    }

    private void InitItemShop(EquipmentType type)
    {
        List<EquipmentData> equipmentDataList = equipmentSODatas.GetSOData(type).GetList();

        foreach (EquipmentData equipment in equipmentDataList)
        {
            WeaponItemShop createdItem = Instantiate(weaponPrefab, content);
            createdItem.Init(equipment.sprite, type, equipment.id, equipment.price);
            items.Add(createdItem);
            createdItem.Despawn();
        }
    }

    private void DisplayWeapon(int index)
    {
        this.index = index;

        currentItemOnShow = (WeaponItemShop) items[index];
        currentItemOnShow.Spawn();
    }
    
    private void NonDisplayWeapon()
    {
        currentItemOnShow.Despawn();
    }

    public void PrevWeapon()
    {
        if (index > 0)
        {
            NonDisplayWeapon();
            DisplayWeapon(index - 1);
            CheckButtonState();
        }
    }

    public void NextWeapon()
    {
        if (index < items.Count - 1)
        {
            NonDisplayWeapon();
            DisplayWeapon(index + 1);
            CheckButtonState();
        }
    }

    public void Buy()
    {
        EquipmentType type = currentItemOnShow.Type;
        EquipmentId id = currentItemOnShow.Id;
        int price = currentItemOnShow.Price;

        if (data.coin >= price)
        {
            data.coin -= price;
            coinText.text = data.coin.ToString();

            data.equipmentBought.Add(id);
            data.equippedEquipment[(int)type] = id;
            UserDataManager.Ins.SaveData();
            CheckButtonState();
        }
        else
        {
            Instantiate(notificationPrefab, notificationHolder.transform);
        }
    }

    public void Equip()
    {
        EquipmentId id = currentItemOnShow.Id;

        data.equippedEquipment[(int)EquipmentType.Weapon] = id;
        UserDataManager.Ins.SaveData();

        CheckButtonState();
    }
    
    public void ReturnHome()
    {
        NonDisplayWeapon();

        LevelManager.Ins.OnMainMenu();
    }
}
