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
    [SerializeField] private Image costumePrefab;

    [Header("Button_Category")]
    [SerializeField] private RectTransform skinButtonSelected;
    [SerializeField] private RectTransform skinButtonNonSelect;
    [SerializeField] private RectTransform hatButtonSelected;
    [SerializeField] private RectTransform hatButtonNonSelect;
    [SerializeField] private RectTransform pantButtonSelected;
    [SerializeField] private RectTransform pantButtonNonSelect;

    [Header("Button_Buy/Equip/Equipped")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject equippedButton;

    [Header("Notification")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationHolder;

    // In Editor
    [HideInInspector] public CostumeShopState currentShopState;

    private List<Image> activatedSkinImages = new List<Image>();
    private List<Image> activatedHatImages = new List<Image>();
    private List<Image> activatedPantImages = new List<Image>();

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

        TriggerSkinShop();

        currentShopState = CostumeShopState.SkinShop;

        OnCategoryIsSelected();
    }

    public void OnClick()
    {
        UserData data = UserDataManager.instance.userData;

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
                        Image createdImage = Instantiate(costumePrefab, content);

                        createdImage.sprite = skinImages[i];

                        Item item = createdImage.GetComponent<Item>();

                        item.costumeShopScript = this;

                        item.id = i;

                        item.equipmentType = EquipmentType.Skin;

                        activatedSkinImages.Add(createdImage);
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
                        Image createdImage = Instantiate(costumePrefab, content);

                        createdImage.sprite = hatImages[i];

                        Item item = createdImage.GetComponent<Item>();

                        item.costumeShopScript = this;

                        item.id = i;

                        item.equipmentType = EquipmentType.Hat;

                        activatedHatImages.Add(createdImage);
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
                        Image createdImage = Instantiate(costumePrefab, content);

                        createdImage.sprite = pantImages[i];

                        Item item = createdImage.GetComponent<Item>();

                        item.costumeShopScript = this;

                        item.id = i;

                        item.equipmentType = EquipmentType.Pant;

                        activatedPantImages.Add(createdImage);
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
                foreach (Image image in activatedSkinImages)
                {
                    image.gameObject.SetActive(true);
                }

                break;
            case CostumeShopState.HatShop:
                foreach (Image image in activatedHatImages)
                {
                    image.gameObject.SetActive(true);
                }

                break;
            case CostumeShopState.PantShop:
                foreach (Image image in activatedPantImages)
                {
                    image.gameObject.SetActive(true);
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

                foreach (Image image in activatedSkinImages)
                {
                    image.gameObject.SetActive(false);
                }

                break;
            case CostumeShopState.HatShop:
                hatButtonSelected.gameObject.SetActive(false);
                hatButtonNonSelect.gameObject.SetActive(true);

                foreach (Image image in activatedHatImages)
                {
                    image.gameObject.SetActive(false);
                }

                break;
            case CostumeShopState.PantShop:
                pantButtonSelected.gameObject.SetActive(false);
                pantButtonNonSelect.gameObject.SetActive(true);

                foreach (Image image in activatedPantImages)
                {
                    image.gameObject.SetActive(false);
                }

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

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<CostumeShop>();

        UIManager.instance.OpenUI<MainMenu>();
    }

    public void Buy()
    {
        UserData data = UserDataManager.instance.userData;

        if (data.coin >= price)
        {
            switch (currentShopState)
            {
                case CostumeShopState.SkinShop:
                    data.coin -= price;
                    data.skinIdList.Add(id);
                    data.equippedSkinId = id;
                    UserDataManager.instance.Save();

                    break;
                case CostumeShopState.HatShop:
                    data.coin -= price;
                    data.hatIdList.Add(id);
                    data.equippedHatId = id;
                    UserDataManager.instance.Save();

                    break;
                case CostumeShopState.PantShop:
                    data.coin -= price;
                    data.pantIdList.Add(id);
                    data.equippedPantId = id;
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
        UserData data = UserDataManager.instance.userData;

        switch (currentShopState)
        {
            case CostumeShopState.SkinShop:
                data.equippedSkinId = id;
                UserDataManager.instance.Save();

                break;
            case CostumeShopState.HatShop:
                data.equippedHatId = id;
                UserDataManager.instance.Save();

                break;
            case CostumeShopState.PantShop:
                data.equippedPantId = id;
                UserDataManager.instance.Save();

                break;
        }

        OnClick();
    }
}
