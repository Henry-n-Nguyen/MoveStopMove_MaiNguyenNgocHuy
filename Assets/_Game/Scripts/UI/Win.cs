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
        UserDataManager.instance.userData.currentLevel = currentLevel + 1 > 3 ? 3 : currentLevel + 1;
        UserDataManager.instance.Save();

        earnCoinText.text = "+ " + (GamePlayManager.instance.coinToEarn).ToString();

        CameraManager.instance.TurnOnCamera(CameraState.Win);
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseUI<Win>(0.5f);

        UIManager.instance.OpenUI<MainMenu>();
    }
}
