using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("References")]
    [SerializeField] private GameObject costumeShopCamera;
    [SerializeField] private GameObject mainMenuCamera;

    private List<Weapon> weaponShopList = new List<Weapon>();

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
        costumeShopCamera.SetActive(false);
        mainMenuCamera.SetActive(false);
    }

    public void TurnOnCamera(CameraState state)
    {
        switch (state)
        {
            case CameraState.MainCamera: OnInit(); break;
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
