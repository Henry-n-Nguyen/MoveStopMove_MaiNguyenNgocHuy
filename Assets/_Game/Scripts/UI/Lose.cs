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
        earnCoinText.text = "+ " + (GamePlayManager.instance.coinToEarn).ToString();

        rankText.text = "#" + GamePlayManager.instance.aliveCharacterAmount.ToString();

        GamePlayManager.instance.currentGamePlayState = GamePlayState.Lose;

        CameraManager.instance.TurnOnCamera(CameraState.Lose);
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<Lose>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
