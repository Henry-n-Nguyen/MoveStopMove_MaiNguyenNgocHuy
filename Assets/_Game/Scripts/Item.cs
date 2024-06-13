using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
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
                costumeShopScript.price = 800;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Skin:
                player.Equip(equipmentType, equipmentDataSO.GetSkinById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 500;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Pant:
                player.Equip(equipmentType, equipmentDataSO.GetPantById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 400;

                costumeShopScript.itemOnClicked = this;

                break;
            case EquipmentType.Special:
                player.DeEquipSpecial();
                player.Equip(equipmentType, equipmentDataSO.GetSpecialById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 5000;

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
