using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Button button;

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
        switch (equipmentType)
        {
            case EquipmentType.Hat:
                player.Equip(equipmentType, EquipmentManager.instance.GetHatById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 800;

                break;
            case EquipmentType.Skin:
                player.Equip(equipmentType, MaterialManager.instance.GetSkinById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 500;

                break;
            case EquipmentType.Pant:
                player.Equip(equipmentType, MaterialManager.instance.GetPantById(id));
                costumeShopScript.id = id;
                costumeShopScript.price = 400;

                break;
            case EquipmentType.Special:
                player.DeEquipSpecial();
                player.Equip(equipmentType, SpecialManager.instance.GetSpecialById(id));

                costumeShopScript.id = id;
                costumeShopScript.price = 5000;

                break;
        }

        costumeShopScript.OnClick();
    }
}
