using System.Collections;
using UnityEngine;

namespace HuySpace
{
    public enum Direct
    {
        Forward,
        ForwardRight,
        Right,
        BackRight,
        Back,
        BackLeft,
        Left,
        ForwardLeft,
        None = -1
    }

    public enum EquipmentType
    {
        Weapon,
        Hat,
        Pant,
        Skin,
        Special
    }

    public enum BoostType
    {
        CoinBoost,
        SpeedBoost,
        EnormousBoost
    }

    public enum CameraState
    {
        MainCamera,
        CostumeShop,
        MainMenu,
        Win,
        Lose
    }

    public enum GamePlayState
    {
        None = -1,
        Loading,
        MainMenu,
        Shop,
        Ingame,
        Win,
        Lose
    }

    public enum CostumeShopState
    {
        SkinShop,
        HatShop,
        PantShop,
        SpecialShop
    }

    public enum ButtonType
    {
        BuyButton,
        EquipButton,
        EquippedButton
    }
}