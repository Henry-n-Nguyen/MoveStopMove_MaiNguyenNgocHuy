using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("CustomShop Camera")]
    [SerializeField] private GameObject costumeShopCamera;

    [Header("Face-to-Face Camera")]
    [SerializeField] private GameObject faceToFaceCamera;

    [Header("Reviev Camera")]
    [SerializeField] private GameObject reviveCamera;
    [SerializeField] private Transform reviveCameraTransform;

    private Vector3 reviveCamOffset;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Setup();
        OnInit();
    }

    private void Setup()
    {
        reviveCamOffset = Vector3.up * 1 + Vector3.forward * 7;
    }

    public void OnInit()
    {
        costumeShopCamera.SetActive(false);
        faceToFaceCamera.SetActive(false);
        reviveCamera.SetActive(false);
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
                faceToFaceCamera.SetActive(true);
                break;
            case CameraState.Revive:
                OnInit();
                reviveCameraTransform.position = GamePlayManager.instance.player.characterTransform.position + reviveCamOffset;
                reviveCamera.SetActive(true);
                break;
        }
    }
}
