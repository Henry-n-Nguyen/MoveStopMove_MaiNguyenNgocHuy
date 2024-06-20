using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    private const int COIN_CONVERT_RATE = 20;
    private const int COIN_BONUS = 100;

    public int characterAmount;

    [HideInInspector] public GamePlayState currentGamePlayState;

    [HideInInspector] public Player player;

    [HideInInspector] public int aliveCharacterAmount;
    [HideInInspector] public int startCharacterAmount = 8;

    // Coin
    [HideInInspector] public int coinToEarn;

    // private
    private UserData data;
    private bool isDiedBefore = false;

    // Event
    public event Action OnAliveCharacterAmountChanged;
    public event Action OnUIChanged;
    public event Action OnPlayerTouchScreen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnInit();

        StartGame();
    }

    public void OnInit()
    {
        StopAllCoroutines();

        data = UserDataManager.instance.userData;

        isDiedBefore = false;

        ChangeState(GamePlayState.None);

        aliveCharacterAmount = characterAmount;

        coinToEarn = 0;

        BotGenerator.instance.characterInQueueToSpawn = startCharacterAmount;
        BotGenerator.instance.characterInBattleAmount = 1;

        BoosterGenerator.instance.OnInit();
    }

    private void StartGame()
    {
        UIManager.instance.OpenUI<MainMenu>();
    }

    public void WinGame()
    {
        coinToEarn = player.point * COIN_CONVERT_RATE + COIN_BONUS;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(TriggerWinStateAfterTime(2.5f));
    }

    private IEnumerator TriggerWinStateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        ChangeState(GamePlayState.None);

        player.OnInit();
        player.Win();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Win>();
    }

    public void LoseGame()
    {
        coinToEarn = player.point * COIN_CONVERT_RATE;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        if (isDiedBefore)
        {
            StartCoroutine(TriggerLoseStateAfterTime(2.5f));
        }
        else
        {
            isDiedBefore = true;

            StartCoroutine(TriggerReviveStateAfterTime(2.5f));
        }
    }

    private IEnumerator TriggerLoseStateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.OnInit();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }

    private IEnumerator TriggerReviveStateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        UIManager.instance.CloseDirectly<DynamicJoyStick>();
        UIManager.instance.OpenUI<Revive>();
    }


    // Event Action
    public void CharacterDied()
    {
        aliveCharacterAmount--;

        OnAliveCharacterAmountChanged?.Invoke();
    }

    public void ChangeState(GamePlayState state)
    {
        currentGamePlayState = state;

        OnUIChanged?.Invoke();
    }

    public void OnTouch()
    {
        OnPlayerTouchScreen?.Invoke();
    }
}
