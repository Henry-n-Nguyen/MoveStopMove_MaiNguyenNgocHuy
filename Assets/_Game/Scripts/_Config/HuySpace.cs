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
        WeaponShop,
        CostumeShop,
        MainMenu
    }
}