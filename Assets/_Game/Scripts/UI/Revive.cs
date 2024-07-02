using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Revive : UICanvas
{
    [SerializeField] private ReviveUIConfigSO config;

    // Coroutines
    private Coroutine delayCoroutine;

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
        timer = config.WaitingTime;

        GamePlayManager.instance.ChangeState(GamePlayState.None);

        CameraManager.instance.TurnOnCamera(CameraState.Revive);
    }

    public void WatchAd()
    {
        GamePlayManager.instance.ChangeStateAfterTime(GamePlayState.Ingame, config.DelayTime);

        GamePlayManager.instance.player.Revive();

        UIManager.instance.CloseUI<Revive>(0);

        UIManager.instance.OpenUI<DynamicJoyStick>();
    }

    public void NoThanks()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.Lose);
    }
}
