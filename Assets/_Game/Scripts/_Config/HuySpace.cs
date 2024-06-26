﻿using System.Collections;
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
        HugeBulletBoost,
        EnormousBoost,
        None = -1
    }

    public enum CameraState
    {
        MainCamera,
        CostumeShop,
        MainMenu,
        Win,
        Lose,
        Revive
    }

    public enum GamePlayState
    {
        None,
        CostumeShop,
        WeaponShop,
        Ingame,
        Win,
        Lose,
        MainMenu,
        Award,
        Settings
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

    public enum ParticleType
    {
        None = -1,
        HitVFX,
        BoostedVFX
    }
}