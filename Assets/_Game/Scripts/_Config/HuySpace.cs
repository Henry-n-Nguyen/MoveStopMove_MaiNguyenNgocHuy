using System.Collections;
using UnityEngine;

namespace HuySpace
{
    public enum EquipmentType { Weapon, Hat, Skin, Pant, Accessory }

    public enum CameraState { Ingame, CostumeShop, MainMenu, Win, Lose, Revive }

    public enum GameState { None, CostumeShop, WeaponShop, Ingame, Win, Lose, MainMenu, Award, Settings }

    public enum CostumeShopState { HatShop, FaceShop, ClothesShop, ShoeShop }

    public enum ButtonType { BuyButton, EquipButton, EquippedButton, LockedButton }

    public enum BotType { AggressiveBot, PatrolBot }

    public enum EquipmentId
    {
        Weapon_Battle_Axe = PoolType.Bullet_Battle_Axe,
        Weapon_Firebrigade_Axe = PoolType.Bullet_Firebrigade_Axe,
        Weapon_Red_Axe = PoolType.Bullet_Red_Axe,
        Weapon_Short_Axe = PoolType.Bullet_Short_Axe,
        Weapon_Steel_Hatchet = PoolType.Bullet_Steel_Hatchet,

        Hat_Arrow = 20,
        Hat_Beard = 21,
        Hat_Cowboy = 22,
        Hat_Crown = 23,
        Hat_Ear = 23,
        Hat_Hat = 24,
        Hat_Cap = 25,
        Hat_Headphone = 26,
        Hat_Straw = 27,

        Skin_Normal = 40,
        Skin_Blue = 41,
        Skin_Green = 42,
        Skin_Pink = 43,
        Skin_Purple = 44,
        Skin_Red = 45,
        Skin_Yellow = 46,
        Skin_Pale = 47,
        Skin_Zom = 48,

        Pant_Batman = 60,
        Pant_Chambi = 61,
        Pant_America = 62,
        Pant_Da_Bao = 63,
        Pant_Onion = 64,
        Pant_Pokemon = 65,
        Pant_RainBow = 66,
        Pant_Skull = 67,
        Pant_Van_tim = 68,

        Accessory_Angel_Bow = 80,
        Accessory_Wizard_Book = 81,
    }

    public enum SkinId
    {
        
    }

    public enum PantId
    {
        
    }

    public enum HatId
    {
        
    }

    public enum AccessoryId
    {
        
    }

    public enum PoolType
    {
        None = 0,

        Bullet_Battle_Axe = 1,
        Bullet_Firebrigade_Axe = 2,
        Bullet_Red_Axe = 3,
        Bullet_Short_Axe = 4,
        Bullet_Steel_Hatchet = 5,
        
        Bot = 20,

        Particle_HitVFX = 40,
        Particle_BoostedVFX = 41,

        Booster_Enormous = 60,
        Booster_HugeBullet = 61,

        PickUp_Battle_Axe = 80,
        PickUp_Firebrigade_Axe = 81,
        PickUp_Red_Axe = 82,
        PickUp_Short_Axe = 83,
        PickUp_Steel_Hatchet = 84,
    }

    public enum BoostType
    {
        None,
        EnormousBoost = PoolType.Booster_Enormous,
        HugeBulletBoost = PoolType.Booster_HugeBullet,
    }

    public enum BulletType
    {
        Bullet_Battle_Axe = PoolType.Bullet_Battle_Axe,
        Bullet_Firebrigade_Axe = PoolType.Bullet_Firebrigade_Axe,
        Bullet_Red_Axe = PoolType.Bullet_Red_Axe,
        Bullet_Short_Axe = PoolType.Bullet_Short_Axe,
        Bullet_Steel_Hatchet = PoolType.Bullet_Steel_Hatchet,
    }
}