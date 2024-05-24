using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("References")]
    [SerializeField] private GameObject costumeShopCamera;
    [SerializeField] private GameObject FaceToFaceCamera;

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
        FaceToFaceCamera.SetActive(false);
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
            case CameraState.Win:
            case CameraState.Lose:
                OnInit();
                FaceToFaceCamera.SetActive(true);
                break;
        }
    }
}
