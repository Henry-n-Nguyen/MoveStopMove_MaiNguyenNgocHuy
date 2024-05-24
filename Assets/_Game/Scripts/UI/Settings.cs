using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Settings : UICanvas
{
    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.MainMenu;
    }

    public void ResumeGame()
    {
        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
        UIManager.instance.OpenUI<Ingame>();
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
