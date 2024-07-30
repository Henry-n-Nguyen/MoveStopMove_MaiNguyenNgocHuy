using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class CameraManager : Singleton<CameraManager>
{
    [field :SerializeField] public CameraFollow InGameCam { get; private set; }

    [SerializeField] private GameObject dynamicCam;
    [SerializeField] private Transform dynamicCamTF;

    private void Start()
    {
        OnInit();
    }

    private void SetupCamera(Vector3 pos, Quaternion rot)
    {
        dynamicCam.SetActive(true);
        dynamicCamTF.position = pos;
        dynamicCamTF.rotation = rot;
    }

    private void OnInit()
    {
        dynamicCam.SetActive(false);
    }

    public void ChangeCameraState(CameraState state)
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        switch (state)
        {
            case CameraState.Ingame:
                OnInit();
                break;
            case CameraState.CostumeShop:
                pos = Vector3.up * 1f + Vector3.back * 8f;
                rot = Quaternion.Euler(Vector3.right * 22);
                SetupCamera(pos, rot);
                break;
            case CameraState.MainMenu:
                pos = Vector3.up * 1f + Vector3.back * 7f;
                rot = Quaternion.Euler(Vector3.right * 10);
                SetupCamera(pos, rot);
                break;
            case CameraState.Win: 
            case CameraState.Lose:
            case CameraState.Revive:
                pos = Vector3.up * 2f + Vector3.back * 8f;
                rot = Quaternion.Euler(Vector3.right * 20);
                SetupCamera(pos, rot);
                break;
        }
    }

}
