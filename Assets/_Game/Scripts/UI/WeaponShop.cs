using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    [Header("Pre")]
    [SerializeField] private Image weaponPrefab;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private RectTransform content;

    // In Editor
    private List<Sprite> weaponImages = new List<Sprite>();

    private List<Image> activeWeaponImages = new List<Image>();

    private int id;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Shop;

        id = 0;

        OnIdChanges();

        weaponImages = EquipmentManager.instance.GetWeaponSpriteList();

        foreach (Sprite image in weaponImages)
        {
            Image createdImage = Instantiate(weaponPrefab, content);

            createdImage.sprite = image;

            activeWeaponImages.Add(createdImage);
        }
    }

    private void OnIdChanges()
    {
        coinText.text = ((id + 1) * 500f).ToString();
    }

    public void PrevWeapon()
    {
        if (id > 0)
        {
            id--;
            content.position += Vector3.right * 650f;
            OnIdChanges();
        }
    }

    public void NextWeapon()
    {
        if (id < weaponImages.Count - 1)
        {
            id++;
            content.position -= Vector3.right * 650f;
            OnIdChanges();
        }
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<WeaponShop>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
