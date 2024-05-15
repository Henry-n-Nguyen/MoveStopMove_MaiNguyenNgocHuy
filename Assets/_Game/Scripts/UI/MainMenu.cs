using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class MainMenu : UICanvas
{
    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        CameraManager.instance.TurnOnCamera(CameraState.MainMenu);
    }

    public void PlayGame()
    {
        UIManager.instance.CloseDirectly<MainMenu>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
        UIManager.instance.OpenUI<Ingame>();

        GamePlayManager.instance.ResumeGame();
    }
}
