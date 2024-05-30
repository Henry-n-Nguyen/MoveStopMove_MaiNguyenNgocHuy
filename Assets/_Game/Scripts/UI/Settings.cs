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
        Time.timeScale = 0.0f;

        GamePlayManager.instance.ChangeState(GamePlayState.None);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;

        GamePlayManager.instance.ChangeState(GamePlayState.Ingame);

        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
    }

    public void ReturnHome()
    {
        Time.timeScale = 1.0f;

        UIManager.instance.CloseDirectly<Settings>();
        UIManager.instance.CloseDirectly<Ingame>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
