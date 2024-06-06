using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lose : UICanvas
{
    [SerializeField] private TextMeshProUGUI earnCoinText;
    [SerializeField] private TextMeshProUGUI rankText;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        BotPool.Collect();

        earnCoinText.text = "+ " + (GamePlayManager.instance.coinToEarn).ToString();

        rankText.text = "#" + GamePlayManager.instance.aliveCharacterAmount.ToString();

        GamePlayManager.instance.player.OnInit();

        GamePlayManager.instance.ChangeState(GamePlayState.None);

        CameraManager.instance.TurnOnCamera(CameraState.Lose);
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseUI<Lose>(0.5f);

        UIManager.instance.OpenUI<MainMenu>();
    }
}
