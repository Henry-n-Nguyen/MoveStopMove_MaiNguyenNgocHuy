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

    private void Update()
    {
        timer -= Time.deltaTime;

        counterText.text = Mathf.Round(timer).ToString();

        if (timer <= 0)
        {
            NoThanks();
        }
    }

    public override void Setup()
    {
        base.Setup();

        timer = config.WaitingTime;
    }

    public void WatchAd()
    {
        CharacterManager.Ins.player.Revive();
        LevelManager.Ins.OnPlay();
    }

    public void NoThanks()
    {
        LevelManager.Ins.OnLose();
    }
}
