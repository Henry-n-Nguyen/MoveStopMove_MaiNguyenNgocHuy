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

        int rank = GamePlayManager.instance.aliveCharacterAmount;
        rankText.text = "#" + rank.ToString();

        int highestRank = UserDataManager.instance.userData.currentHighestRank;
        if (rank < highestRank)
        {
            UserDataManager.instance.userData.currentHighestLevel = UserDataManager.instance.userData.currentLevel + 1;
            UserDataManager.instance.userData.currentHighestRank = rank;
            UserDataManager.instance.Save();
        }

        GamePlayManager.instance.player.OnInit();

        CameraManager.instance.TurnOnCamera(CameraState.Lose);
    }

    public void ReturnHome()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.MainMenu);
    }

    public void TripleAward()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.Award);
    }
}
