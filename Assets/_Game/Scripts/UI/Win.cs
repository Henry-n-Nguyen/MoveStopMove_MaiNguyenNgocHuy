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
        earnCoinText.text = "+ " + GamePlayManager.instance.coinToEarn.ToString();

        GamePlayManager.instance.currentGamePlayState = GamePlayState.Win;

        CameraManager.instance.TurnOnCamera(CameraState.Win);
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<Win>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
