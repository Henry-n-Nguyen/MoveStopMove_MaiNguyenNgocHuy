using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using HuySpace;

public class CostumeShop : UICanvas
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RectTransform content;
    [SerializeField] private Item costumePrefab;

    [Header("Button_Category")]
    [SerializeField] private RectTransform skinButtonSelected;
    [SerializeField] private RectTransform skinButtonNonSelect;
    [SerializeField] private RectTransform hatButtonSelected;
    [SerializeField] private RectTransform hatButtonNonSelect;
    [SerializeField] private RectTransform pantButtonSelected;
    [SerializeField] private RectTransform pantButtonNonSelect;
    [SerializeField] private RectTransform specialButtonSelected;
    [SerializeField] private RectTransform specialButtonNonSelect;

    [Header("Button_Buy/Equip/Equipped")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject equippedButton;

    [Header("Notification")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationHolder;

    // In Editor
    [HideInInspector] public CostumeShopState currentShopState;

    private List<Item> activatedSkinImages = new List<Item>();
    private List<Item> activatedHatImages = new List<Item>();
    private List<Item> activatedPantImages = new List<Item>();
    private List<Item> activatedSpecialImages = new List<Item>();

    private UserData data;

    private bool isEquippedSpecial;

    [HideInInspector] public int id;
    [HideInInspector] public int price;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Shop;

        CameraManager.instance.TurnOnCamera(CameraState.CostumeShop);

        data = UserDataManager.instance.userData;

        coinText.text = data.coin.ToString();

        isEquippedSpecial = data.isSpecialEquipped;
        GamePlayManager.instance.player.DeEquipSpecial();
        data.isSpecialEquipped = false;
        GamePlayManager.instance.player.LoadDataFromUserData();

        TriggerSkinShop();

        currentShopState = CostumeShopState.SkinShop;

        OnCategoryIsSelected();
    }

    public void OnClick()
    {
        coinText.text = data.coin.ToString();

        switch (currentShopState)
        {
            case CostumeShopState.SkinShop:
                if (data.skinIdList.IndexOf(id) == -1)
                {
                    ChangeButton(ButtonType.BuyButton);

                    priceText.text = price.ToString();
                }
                else
                {
                    if (id == data.equippedSkinId)
                    {
                        ChangeButton(ButtonType.EquippedButton);
                    }
                    else
                    {
                        ChangeButton(ButtonType.EquipButton);
                    }
                }

                break;
            case CostumeShopState.HatShop:
                if (data.hatIdList.IndexOf(id) == -1)
                {
                    ChangeButton(ButtonType.BuyButton);

                    priceText.text = price.ToString();
                }
                else
                {
                    if (id == data.equippedHatId)
                    {
                        ChangeButton(ButtonType.EquippedButton);
                    }
                    else
                    {
                        ChangeButton(ButtonType.EquipButton);
                    }
                }

                break;
            case CostumeShopState.PantShop:
                if (data.pantIdList.IndexOf(id) == -1)
                {
                    ChangeButton(ButtonType.BuyButton);

                    priceText.text = price.ToString();
                }
                else
                {
                    if (id == data.equippedPantId)
                    {
                        ChangeButton(ButtonType.EquippedButton);
                    }
                    else
                    {
                        ChangeButton(ButtonType.EquipButton);
                    }
                }

                break;
            case CostumeShopState.SpecialShop:
                if (data.specialIdList.IndexOf(id) == -1)
                {
                    ChangeButton(ButtonType.BuyButton);

                    priceText.text = price.ToString();
                }
                else
                {
                    if (id == data.equippedSpecialId && isEquippedSpecial)
                    {
                        ChangeButton(ButtonType.EquippedButton);
                    }
                    else
                    {
                        ChangeButton(ButtonType.EquipButton);
                    }
                }

                break;
        }
    }

    private void ChangeButton(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.BuyButton:
                buyButton.SetActive(true);
                equipButton.SetActive(false);
                equippedButton.SetActive(false);

                break;
            case ButtonType.EquipButton:
                buyButton.SetActive(false);
                equipButton.SetActive(true);
                equippedButton.SetActive(false);

                break;
            case ButtonType.EquippedButton:
                buyButton.SetActive(false);
                equipButton.SetActive(false);
                equippedButton.SetActive(true);

                break;
        }
    }

    private void OnCategoryIsSelected()
    {
        switch (currentShopState)
        {
            case CostumeShopState.SkinShop:
                skinButtonSelected.gameObject.SetActive(true);
                skinButtonNonSelect.gameObject.SetActive(false);

                if (activatedSkinImages.Count > 0)
                {
                    ActiveState(currentShopState);
                }
                else {
                    List<Sprite> skinImages = MaterialManager.instance.GetSkinSpriteList();
                    for (int i = 0; i < skinImages.Count; i++)
                    {
                        Item createdItem = Instantiate(costumePrefab, content);

                        createdItem.icon.sprite = skinImages[i];

                        createdItem.costumeShopScript = this;

                        createdItem.id = i;

                        createdItem.equipmentType = EquipmentType.Skin;

                        activatedSkinImages.Add(createdItem);
                    }
                }

                break;
            case CostumeShopState.HatShop:
                hatButtonSelected.gameObject.SetActive(true);
                hatButtonNonSelect.gameObject.SetActive(false);

                if (activatedHatImages.Count > 0)
                {
                    ActiveState(currentShopState);
                }
                else
                {
                    List<Sprite> hatImages = EquipmentManager.instance.GetHatSpriteList();
                    for (int i = 0; i < hatImages.Count; i++)
                    {
                        Item createdItem = Instantiate(costumePrefab, content);

                        createdItem.icon.sprite = hatImages[i];

                        createdItem.costumeShopScript = this;

                        createdItem.id = i;

                        createdItem.equipmentType = EquipmentType.Hat;

                        activatedHatImages.Add(createdItem);
                    }
                }

                break;
            case CostumeShopState.PantShop:
                pantButtonSelected.gameObject.SetActive(true);
                pantButtonNonSelect.gameObject.SetActive(false);

                if (activatedPantImages.Count > 0)
                {
                    ActiveState(currentShopState);
                }
                else
                {
                    List<Sprite> pantImages = MaterialManager.instance.GetPantSpriteList();
                    for (int i = 0; i < pantImages.Count; i++)
                    {
                        Item createdItem = Instantiate(costumePrefab, content);

                        createdItem.icon.sprite = pantImages[i];

                        createdItem.costumeShopScript = this;

                        createdItem.id = i;

                        createdItem.equipmentType = EquipmentType.Pant;

                        activatedPantImages.Add(createdItem);
                    }
                }

                break;
            case CostumeShopState.SpecialShop:
                specialButtonSelected.gameObject.SetActive(true);
                specialButtonNonSelect.gameObject.SetActive(false);

                if (activatedSpecialImages.Count > 0)
                {
                    ActiveState(currentShopState);
                }
                else
                {
                    List<Sprite> specialImages = SpecialManager.instance.GetSpecialSpriteList();
                    for (int i = 0; i < specialImages.Count; i++)
                    {
                        Item createdItem = Instantiate(costumePrefab, content);

                        createdItem.icon.sprite = specialImages[i];

                        createdItem.costumeShopScript = this;

                        createdItem.id = i;

                        createdItem.equipmentType = EquipmentType.Special;

                        activatedSpecialImages.Add(createdItem);
                    }
                }

                break;
        }
    }

    private void ActiveState(CostumeShopState state)
    {
        switch (state)
        {
            case CostumeShopState.SkinShop:
                foreach (Item item in activatedSkinImages)
                {
                    item.gameObject.SetActive(true);
                }

                break;
            case CostumeShopState.HatShop:
                foreach (Item item in activatedHatImages)
                {
                    item.gameObject.SetActive(true);
                }

                break;
            case CostumeShopState.PantShop:
                foreach (Item item in activatedPantImages)
                {
                    item.gameObject.SetActive(true);
                }

                break;
            case CostumeShopState.SpecialShop:
                foreach (Item item in activatedSpecialImages)
                {
                    item.gameObject.SetActive(true);
                }

                break;
        }
    }

    private void DeactiveState(CostumeShopState state)
    {
        switch (state)
        {
            case CostumeShopState.SkinShop:
                skinButtonSelected.gameObject.SetActive(false);
                skinButtonNonSelect.gameObject.SetActive(true);

                foreach (Item item in activatedSkinImages)
                {
                    item.gameObject.SetActive(false);
                }

                break;
            case CostumeShopState.HatShop:
                hatButtonSelected.gameObject.SetActive(false);
                hatButtonNonSelect.gameObject.SetActive(true);

                foreach (Item item in activatedHatImages)
                {
                    item.gameObject.SetActive(false);
                }

                break;
            case CostumeShopState.PantShop:
                pantButtonSelected.gameObject.SetActive(false);
                pantButtonNonSelect.gameObject.SetActive(true);

                foreach (Item item in activatedPantImages)
                {
                    item.gameObject.SetActive(false);
                }

                break;
            case CostumeShopState.SpecialShop:
                specialButtonSelected.gameObject.SetActive(false);
                specialButtonNonSelect.gameObject.SetActive(true);

                foreach (Item item in activatedSpecialImages)
                {
                    item.gameObject.SetActive(false);
                }

                GamePlayManager.instance.player.DeEquipSpecial();
                data.isSpecialEquipped = false;
                GamePlayManager.instance.player.LoadDataFromUserData();

                break;
        }
    }

    public void TriggerSkinShop()
    {
        DeactiveState(currentShopState);

        currentShopState = CostumeShopState.SkinShop;

        OnCategoryIsSelected();
    }

    public void TriggerHatShop()
    {
        DeactiveState(currentShopState);

        currentShopState = CostumeShopState.HatShop;

        OnCategoryIsSelected();
    }

    public void TriggerPantShop()
    {
        DeactiveState(currentShopState);

        currentShopState = CostumeShopState.PantShop;

        OnCategoryIsSelected();
    }

    public void TriggerSpecialShop()
    {
        DeactiveState(currentShopState);

        currentShopState = CostumeShopState.SpecialShop;

        OnCategoryIsSelected();
    }

    public void ReturnHome()
    {
        data.isSpecialEquipped = isEquippedSpecial;
        UserDataManager.instance.Save();

        UIManager.instance.CloseDirectly<CostumeShop>();

        UIManager.instance.OpenUI<MainMenu>();
    }

    public void Buy()
    {
        if (data.coin >= price)
        {
            switch (currentShopState)
            {
                case CostumeShopState.SkinShop:
                    isEquippedSpecial = false;

                    data.coin -= price;
                    data.skinIdList.Add(id);
                    data.equippedSkinId = id;
                    UserDataManager.instance.Save();

                    break;
                case CostumeShopState.HatShop:
                    isEquippedSpecial = false;

                    data.coin -= price;
                    data.hatIdList.Add(id);
                    data.equippedHatId = id;
                    UserDataManager.instance.Save();

                    break;
                case CostumeShopState.PantShop:
                    isEquippedSpecial = false;

                    data.coin -= price;
                    data.pantIdList.Add(id);
                    data.equippedPantId = id;
                    UserDataManager.instance.Save();

                    break;
                case CostumeShopState.SpecialShop:
                    isEquippedSpecial = true;

                    data.coin -= price;
                    data.specialIdList.Add(id);
                    data.equippedSpecialId = id;
                    UserDataManager.instance.Save();

                    break;
            }
            

            OnClick();
        }
        else
        {
            Instantiate(notificationPrefab, notificationHolder.transform);
        }
    }

    public void Equip()
    {
        switch (currentShopState)
        {
            case CostumeShopState.SkinShop:
                isEquippedSpecial = false;

                data.equippedSkinId = id;
                UserDataManager.instance.Save();

                break;
            case CostumeShopState.HatShop:
                isEquippedSpecial = false;

                data.equippedHatId = id;
                UserDataManager.instance.Save();

                break;
            case CostumeShopState.PantShop:
                isEquippedSpecial = false;

                data.equippedPantId = id;
                UserDataManager.instance.Save();

                break;
            case CostumeShopState.SpecialShop:
                isEquippedSpecial = true;

                data.equippedSpecialId = id;
                UserDataManager.instance.Save();

                break;
        }

        OnClick();
    }
}
