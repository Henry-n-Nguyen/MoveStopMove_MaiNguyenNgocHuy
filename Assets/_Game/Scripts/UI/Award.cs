using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HuySpace;

public class Award : UICanvas
{
    private const float MULTIPLIER = 3.0f;

    [SerializeField] private TextMeshProUGUI earnCoinText;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        // FIX : define COIN_TO_EARN
        //int coinToEarn = GamePlayManager.Ins.coinToEarn;
        //earnCoinText.text = "+ " + (Mathf.RoundToInt(coinToEarn * MULTIPLIER)).ToString();
        //UserDataManager.Ins.userData.coin += Mathf.RoundToInt(coinToEarn * (MULTIPLIER - 1));
    }

    public void ReturnHome()
    {
        GamePlayManager.Ins.ChangeState(GameState.MainMenu);
    }
}
