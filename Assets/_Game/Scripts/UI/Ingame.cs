using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ingame : UICanvas
{
    [SerializeField] private TextMeshProUGUI aliveCharacterText;
    [SerializeField] private GameObject helpToMovePart;

    // Coroutines
    private Coroutine spawnBoosterCoroutine;

    private int temporaryAliveCharacter;

    private void Start()
    {
        SubscribeEvent();
    }

    private void OnEnable()
    {
        OnInit();
    }

    private void SubscribeEvent()
    {
        GamePlayManager.instance.OnAliveCharacterAmountChanged += UpdateCounter;
        GamePlayManager.instance.OnPlayerTouchScreen += NonDisplayHelpToMovePart;
    }

    private void OnInit()
    {
        StartCoroutine(Loading(4f));

        helpToMovePart.SetActive(true);

        aliveCharacterText.text = "Alive : " + GamePlayManager.instance.aliveCharacterAmount.ToString();

        CameraManager.instance.TurnOnCamera(CameraState.MainCamera);

        spawnBoosterCoroutine = StartCoroutine(SpawnBoosterAfterTime(15f));
    }

    private void OnDisable()
    {
        StopCoroutine(spawnBoosterCoroutine);
    }

    private IEnumerator SpawnBoosterAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        BoosterGenerator.instance.SpawnBooster(1);
    }

    public void UpdateCounter()
    {
        aliveCharacterText.text = "Alive : " + GamePlayManager.instance.aliveCharacterAmount.ToString();
        if (GamePlayManager.instance.aliveCharacterAmount == 1) GamePlayManager.instance.ChangeState(GamePlayState.Win);
    }

    public void NonDisplayHelpToMovePart()
    {
        helpToMovePart.SetActive(false);
    }

    public void OpenSettings()
    {
        GamePlayManager.instance.ChangeState(GamePlayState.Settings);
    }

    private IEnumerator Loading(float time)
    {
        UIManager.instance.OpenUI<Loading>();

        yield return new WaitForSeconds(time);

        UIManager.instance.CloseUI<Loading>(0);

        GamePlayManager.instance.ChangeState(GamePlayState.Ingame);
    }
}
