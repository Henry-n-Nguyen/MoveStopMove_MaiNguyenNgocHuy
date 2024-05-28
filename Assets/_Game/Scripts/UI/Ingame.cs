using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ingame : UICanvas
{
    [SerializeField] private TextMeshProUGUI aliveCharacterText;

    private int temporaryAliveCharacter;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Ingame;

        aliveCharacterText.text = "Alive : " + GamePlayManager.instance.aliveCharacterAmount.ToString();

        CameraManager.instance.TurnOnCamera(CameraState.MainCamera);

        StartCoroutine(SpawnBoosterAfterTime(15f));
    }

    private IEnumerator SpawnBoosterAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        BoosterGenerator.instance.SpawnBooster(1);
    }

    public void UpdateCounter()
    {
        aliveCharacterText.text = "Alive : " + GamePlayManager.instance.aliveCharacterAmount.ToString();
        if (GamePlayManager.instance.aliveCharacterAmount == 1) GamePlayManager.instance.WinGame();
    }

    public void OpenSettings()
    {
        UIManager.instance.CloseDirectly<DynamicJoyStick>();
        UIManager.instance.CloseDirectly<Ingame>();

        UIManager.instance.OpenUI<Settings>();
    }
}
