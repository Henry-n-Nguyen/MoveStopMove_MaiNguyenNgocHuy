using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    const string TRIGGER_FLYTEXT = "fly";

    [Header("Pre")]
    [SerializeField] private Item weaponPrefab;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RectTransform content;

    [Header("Buttons")]
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject equippedButton;

    [Header("Notification")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationHolder;

    // In Editor
    private List<Sprite> weaponImages = new List<Sprite>();

    private List<Item> activeWeaponImages = new List<Item>();

    private int index;
    private int price;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Shop;

        index = 0;

        OnChanges();

        weaponImages = EquipmentManager.instance.GetWeaponSpriteList();

        foreach (Sprite image in weaponImages)
        {
            Item createdItem = Instantiate(weaponPrefab, content);

            createdItem.icon.sprite = image;

            activeWeaponImages.Add(createdItem);
        }
    }

    private void OnChanges()
    {
        UserData data = UserDataManager.instance.userData;

        coinText.text = data.coin.ToString();

        if (data.weaponIdList.IndexOf(index) == -1)
        {
            ChangeButton(ButtonType.BuyButton);

            price = ((index + 1) * 500);
            priceText.text = price.ToString();
        }
        else
        {
            if (index == data.equippedWeaponId)
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

    public void PrevWeapon()
    {
        if (index > 0)
        {
            index--;
            content.position += Vector3.right * 650f;
            OnChanges();
        }
    }

    public void NextWeapon()
    {
        if (index < weaponImages.Count - 1)
        {
            index++;
            content.position -= Vector3.right * 650f;
            OnChanges();
        }
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<WeaponShop>();

        UIManager.instance.OpenUI<MainMenu>();
    }

    public void Buy()
    {
        UserData data = UserDataManager.instance.userData;

        if (data.coin >= price)
        {
            data.coin -= price;
            data.weaponIdList.Add(index);
            data.equippedWeaponId = index;
            UserDataManager.instance.Save();

            OnChanges();
        }
        else
        {
            Instantiate(notificationPrefab, notificationHolder.transform);
        }
    }

    public void Equip()
    {
        UserData data = UserDataManager.instance.userData;

        data.equippedWeaponId = index;
        UserDataManager.instance.Save();

        OnChanges();
    }
}
