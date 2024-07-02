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
        StartCoroutine(Loading(1f));

        int coinToEarn = GamePlayManager.instance.coinToEarn;

        earnCoinText.text = "+ " + (Mathf.RoundToInt(coinToEarn * MULTIPLIER)).ToString();
        UserDataManager.instance.userData.coin += Mathf.RoundToInt(coinToEarn * (MULTIPLIER - 1));
    }

    public void ReturnHome()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.MainMenu);
    }

    private IEnumerator Loading(float time)
    {
        UIManager.instance.OpenUI<Loading>();

        yield return new WaitForSeconds(time);

        UIManager.instance.CloseDirectly<Loading>();
    }
}
