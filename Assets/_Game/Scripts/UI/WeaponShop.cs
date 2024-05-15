using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UICanvas
{
    [SerializeField] private RectTransform content;

    [SerializeField] private List<Image> weaponImages = new List<Image>();
    
    private List<Image> activeWeaponImages = new List<Image>();

    private int id;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        id = 0;

        weaponImages = EquipmentManager.instance.GetWeaponImageList();

        foreach (Image image in weaponImages)
        {
            Image createdImage = Instantiate(image, content);

            activeWeaponImages.Add(createdImage);
        }

    }

    public void PrevWeapon()
    {
        if (id > 0)
        {
            id--;
            content.position += Vector3.right * 650f;
        }
    }

    public void NextWeapon()
    {
        if (id < weaponImages.Count)
        {
            id++;
            content.position -= Vector3.right * 650f;
        }
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<WeaponShop>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
