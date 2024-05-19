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

    private void Update()
    {
        if (GamePlayManager.instance.aliveCharacterAmount < temporaryAliveCharacter)
        {
            temporaryAliveCharacter = GamePlayManager.instance.aliveCharacterAmount;
            aliveCharacterText.text = "Alive : " + temporaryAliveCharacter.ToString();
        }
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Ingame;

        temporaryAliveCharacter = GamePlayManager.instance.aliveCharacterAmount;
        aliveCharacterText.text = "Alive : " + temporaryAliveCharacter.ToString();

        CameraManager.instance.TurnOnCamera(CameraState.MainCamera);
        StartCoroutine(SpawnBoosterAfterTime(15f));
    }

    private IEnumerator SpawnBoosterAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        BoosterGenerator.instance.SpawnBooster(1);
    }

    public void OpenSettings()
    {
        UIManager.instance.CloseDirectly<DynamicJoyStick>();
        UIManager.instance.CloseDirectly<Ingame>();

        UIManager.instance.OpenUI<Settings>();
    }
}
