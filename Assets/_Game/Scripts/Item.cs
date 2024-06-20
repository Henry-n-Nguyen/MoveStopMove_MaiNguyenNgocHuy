using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Default prices
    public const int HAT_PRICE = 800;
    public const int SKIN_PRICE = 500;
    public const int PANT_PRICE = 400;
    public const int SPECIAL_PRICE = 5000;

    [Header("References")]
    [SerializeField] private GameObject itemGameObject;

    [SerializeField] private Button button;
    [SerializeField] private GameObject buttonBorder;

    [Header("EquipmentDataSO")]
    [SerializeField] private EquipmentDataSO equipmentDataSO;

    [Header("Icon")]
    public Image icon;

    [HideInInspector] public int id;

    [HideInInspector] public CostumeShop costumeShopScript;

    [HideInInspector] public EquipmentType equipmentType;

    private Player player;

    private void Start()
    {
        button.onClick.AddListener(OnImageClick);

        player = GamePlayManager.instance.player;
    }

    public void OnImageClick()
    {
        if (costumeShopScript.itemOnClicked != null) costumeShopScript.itemOnClicked.OnClickedAnotherItem();
        OnClickedItem();

        switch (equipmentType)
        {
            case EquipmentType.Hat:
                player.Equip(equipmentType, equipmentDataSO.GetHatById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = HAT_PRICE;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Skin:
                player.Equip(equipmentType, equipmentDataSO.GetSkinById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = SKIN_PRICE;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Pant:
                player.Equip(equipmentType, equipmentDataSO.GetPantById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = PANT_PRICE;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Special:
                player.DeEquipSpecial();
                player.Equip(equipmentType, equipmentDataSO.GetSpecialById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = SPECIAL_PRICE;

                costumeShopScript.itemOnClicked = this;

                break;
        }

        costumeShopScript.OnClick();
    }

    public void Spawn()
    {
        itemGameObject.SetActive(true);
    }

    public void Despawn()
    {
        itemGameObject.SetActive(false);
    }

    public void OnClickedItem()
    {
        buttonBorder.SetActive(true);
    }

    public void OnClickedAnotherItem()
    {
        buttonBorder.SetActive(false);

    }
}
