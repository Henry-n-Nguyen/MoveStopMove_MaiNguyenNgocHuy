using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Win : UICanvas
{
    [SerializeField] private TextMeshProUGUI earnCoinText;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        BotPool.Collect();

        int currentLevel = UserDataManager.instance.userData.currentLevel;
        int currentMaxLevel = LevelManager.instance.GetCurentMaxLevel();

        UserDataManager.instance.userData.currentHighestRank = 1;
        UserDataManager.instance.userData.currentHighestLevel = UserDataManager.instance.userData.currentLevel + 1;
        UserDataManager.instance.userData.currentLevel = currentLevel + 1 > currentMaxLevel ? currentMaxLevel : currentLevel + 1;
        UserDataManager.instance.Save();

        earnCoinText.text = "+ " + (GamePlayManager.instance.coinToEarn).ToString();

        GamePlayManager.instance.player.OnInit();
        GamePlayManager.instance.player.Win();

        CameraManager.instance.TurnOnCamera(CameraState.Win);
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseUI<Win>(0.5f);

        UIManager.instance.OpenUI<MainMenu>();
    }

    public void TripleAward()
    {
        UIManager.instance.CloseUI<Win>(0.5f);

        UIManager.instance.OpenUI<Award>();
    }
}
