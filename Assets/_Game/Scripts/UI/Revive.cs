using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Revive : UICanvas
{
    [SerializeField] private TextMeshProUGUI counterText;

    private float timer = 5.5f;

    private void OnEnable()
    {
        OnInit();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        counterText.text = Mathf.Round(timer).ToString();

        if (timer <= 0)
        {
            NoThanks();
        }
    }

    private void OnInit()
    {
        timer = 5.5f;

        GamePlayManager.instance.ChangeState(GamePlayState.None);

        CameraManager.instance.TurnOnCamera(CameraState.Lose);
    }

    public void WatchAd()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.Ingame);

        GamePlayManager.instance.player.Revive();

        UIManager.instance.CloseDirectly<Revive>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
    }

    public void NoThanks()
    {
        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }
}
