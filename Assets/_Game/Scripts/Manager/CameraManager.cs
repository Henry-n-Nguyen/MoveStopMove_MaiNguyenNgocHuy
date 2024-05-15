using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("References")]
    [SerializeField] private GameObject weaponShopCamera;
    [SerializeField] private GameObject costumeShopCamera;
    [SerializeField] private GameObject mainMenuCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        weaponShopCamera.SetActive(false);
        costumeShopCamera.SetActive(false);
        mainMenuCamera.SetActive(false);
    }

    public void TurnOnCamera(CameraState state)
    {
        switch (state)
        {
            case CameraState.MainCamera: OnInit(); break;
            case CameraState.WeaponShop: 
                OnInit();
                weaponShopCamera.SetActive(true);
                break;
            case CameraState.CostumeShop:
                OnInit();
                costumeShopCamera.SetActive(true);
                break;
            case CameraState.MainMenu:
                OnInit();
                mainMenuCamera.SetActive(true);
                break;
        }
    }
}
