using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using HuySpace;
using System.Diagnostics;

public class CostumeShop : UICanvas
{
    [Header("equipmentSODatas")]
    [SerializeField] private EquipmentSODatas equipmentSODatas;

    [Space(0.3f)]
    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RectTransform content;
    [SerializeField] private CostumeItemShop costumePrefab;

    [Space(0.3f)]
    [Header("Button_Category")]
    [SerializeField] private RectTransform hatButtonSelected;
    [SerializeField] private RectTransform hatButtonNonSelect;

    [SerializeField] private RectTransform accessoryButtonSelected;
    [SerializeField] private RectTransform accessoryButtonNonSelect;

    [SerializeField] private RectTransform pantButtonSelected;
    [SerializeField] private RectTransform pantButtonNonSelect;

    [SerializeField] private RectTransform skinButtonSelected;
    [SerializeField] private RectTransform skinButtonNonSelect;

    [Space(0.3f)]
    [Header("Button_Buy/Equip/Equipped")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject equippedButton;

    [Space(0.3f)]
    [Header("Notification")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationHolder;

    // In Editor
    private EquipmentType currentItemShopType;
    private UserData data;
    private List<EquipmentId> equipmentDataList;

    // Current shop item Clicked
    private CostumeItemShop currentItemOnClicked = null;

    private void Awake()
    {
        data = UserDataManager.Ins.userData;
    }

    public override void Setup()
    {
        base.Setup();

        coinText.text = data.coin.ToString();

        CharacterManager.Ins.player.LoadDataFromUserData();

        TriggerSkinShop();
    }

    public void OnItemClick(CostumeItemShop item)
    {
        if (currentItemOnClicked != null) currentItemOnClicked.OnRelease();

        currentItemOnClicked = item;
        currentItemOnClicked.OnClick();

        switch (item.Type)
        {
            case EquipmentType.Hat:
                CharacterManager.Ins.player.Equip(GetEquipmentPrefab<Hat>(item.Type, item.Id));
                break;
            case EquipmentType.Accessory:
                CharacterManager.Ins.player.Equip(GetEquipmentPrefab<Accessory>(item.Type, item.Id));
                break;
            case EquipmentType.Pant:
                CharacterManager.Ins.player.Equip(GetEquipmentPrefab<Pant>(item.Type, item.Id));
                break;
            case EquipmentType.Skin:
                CharacterManager.Ins.player.Equip(GetEquipmentPrefab<Skin>(item.Type, item.Id));
                break;
        }

        CheckButtonState();
    }

    private T GetEquipmentPrefab<T>(EquipmentType type, EquipmentId id) where T : Equipment
    {
        T item = equipmentSODatas.GetSOData(type).GetData(id).GetPrefab<T>();
        return item;
    }

    private void CheckButtonState()
    {
        EquipmentType type = currentItemOnClicked.Type;
        EquipmentId id = currentItemOnClicked.Id;
        int price = currentItemOnClicked.Price;

        if (!data.equipmentBought.Contains(id))
        {
            ChangeButton(ButtonType.BuyButton);

            priceText.text = price.ToString();
        }
        else
        {
            if (id == data.equippedEquipment[(int)type])
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
        }
    }

    private void OnCategorySelected(EquipmentType type)
    {
        DeactiveItemShop(currentItemShopType);

        currentItemShopType = type;

        if (Cache.GetItemList(currentItemShopType).Count > 0)
        {
            ActiveItemShop(currentItemShopType);
        }
        else
        {
            InitItemShop(currentItemShopType);
        }
    }

    private void InitItemShop(EquipmentType type)
    {
        List<EquipmentData> list = equipmentSODatas.GetSOData(type).GetList();

        foreach (EquipmentData equipmentData in list)
        {
            CostumeItemShop createdItem = Instantiate(costumePrefab, content);
            createdItem.Init(equipmentData.sprite, type, equipmentData.id, equipmentData.price, this);
            Cache.GetItemList(type).Add(createdItem);
        }
    }

    private void ActiveItemShop(EquipmentType type)
    {
        ActiveCategoryButton(type);

        foreach (CostumeItemShop item in Cache.GetItemList(type))
        {
            item.Spawn();
        }
    }
    private void ActiveCategoryButton(EquipmentType type)
    {
        hatButtonSelected.gameObject.SetActive(false);
        hatButtonNonSelect.gameObject.SetActive(true);

        accessoryButtonSelected.gameObject.SetActive(false);
        accessoryButtonNonSelect.gameObject.SetActive(true);

        pantButtonSelected.gameObject.SetActive(false);
        pantButtonNonSelect.gameObject.SetActive(true);

        skinButtonSelected.gameObject.SetActive(false);
        skinButtonNonSelect.gameObject.SetActive(true);

        switch (type)
        {
            case EquipmentType.Hat:
                hatButtonSelected.gameObject.SetActive(true);
                hatButtonNonSelect.gameObject.SetActive(false);
                break;
            case EquipmentType.Accessory:
                accessoryButtonSelected.gameObject.SetActive(true);
                accessoryButtonNonSelect.gameObject.SetActive(false);
                break;
            case EquipmentType.Pant:
                pantButtonSelected.gameObject.SetActive(true);
                pantButtonNonSelect.gameObject.SetActive(false);
                break;
            case EquipmentType.Skin:
                skinButtonSelected.gameObject.SetActive(true);
                skinButtonNonSelect.gameObject.SetActive(false);
                break;
        }
    }

    private void DeactiveItemShop(EquipmentType type)
    {
        foreach (CostumeItemShop item in Cache.GetItemList(type))
        {
            item.Despawn();
        }
    }

    // Change Category
    public void TriggerSkinShop()
    {
        OnCategorySelected(EquipmentType.Skin);
    }

    public void TriggerHatShop()
    {
        OnCategorySelected(EquipmentType.Hat);
    }

    public void TriggerPantShop()
    {
        OnCategorySelected(EquipmentType.Pant);
    }

    public void TriggerAccessoryShop()
    {
        OnCategorySelected(EquipmentType.Accessory);
    }

    // Button Function
    public void Buy()
    {
        EquipmentType type = currentItemOnClicked.Type;
        EquipmentId id = currentItemOnClicked.Id;
        int price = currentItemOnClicked.Price; 

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
        EquipmentType type = currentItemOnClicked.Type;
        EquipmentId id = currentItemOnClicked.Id;

        data.equippedEquipment[(int)type] = id;
        UserDataManager.Ins.SaveData();

        CheckButtonState();
    }
    
    public void ReturnHome()
    {
        LevelManager.Ins.OnMainMenu();
    }
}
