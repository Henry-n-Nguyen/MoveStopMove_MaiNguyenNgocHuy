using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Revive : UICanvas
{
    private const float WAITING_TIME = 5.5f;

    [SerializeField] private TextMeshProUGUI counterText;

    private float timer;

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
        timer = WAITING_TIME;

        CameraManager.instance.TurnOnCamera(CameraState.Revive);
    }

    public void WatchAd()
    {
        GamePlayManager.instance.player.Revive();

        UIManager.instance.CloseUI<Revive>(0);

        UIManager.instance.OpenUI<DynamicJoyStick>();
    }

    public void NoThanks()
    {
        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }
}
