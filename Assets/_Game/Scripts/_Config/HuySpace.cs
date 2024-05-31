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
        HugeBulletBoost,
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
        None,
        Shop,
        Ingame
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
        HitVFX,
        BoostedVFX
    }
}