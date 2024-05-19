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
        Skin
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
        MainMenu
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
}