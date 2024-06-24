using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System;
using UnityEditor;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    private const int COIN_CONVERT_RATE = 20;
    private const int COIN_BONUS = 100;

    public int characterAmount;
    public int startCharacterAmount = 7;

    [HideInInspector] public GamePlayState currentGamePlayState;

    [HideInInspector] public Player player;

    [HideInInspector] public int aliveCharacterAmount;

    // Coin
    [HideInInspector] public int coinToEarn;

    // private
    private UserData data;
    private bool isDiedBefore = false;

    // Event
    public event Action OnAliveCharacterAmountChanged;
    public event Action OnUIChanged;
    public event Action OnPlayerTouchScreen;

    // Coroutines
    private Coroutine triggerWin;
    private Coroutine triggerLose;
    private Coroutine triggerRevive;

    private Coroutine delayCoroutine;

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
        if (triggerWin != null) StopCoroutine(triggerWin);
        if (triggerLose != null) StopCoroutine(triggerLose);
        if (triggerLose != null) StopCoroutine(triggerRevive);

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

        triggerWin = StartCoroutine(TriggerWinStateAfterTime(2.5f));
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
            triggerLose = StartCoroutine(TriggerLoseStateAfterTime(2.5f));
        }
        else
        {
            isDiedBefore = true;

            triggerRevive = StartCoroutine(TriggerReviveStateAfterTime(2.5f));
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
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);

        currentGamePlayState = state;

        OnUIChanged?.Invoke();
    }

    public void ChangeStateAfterTime(GamePlayState state, float time)
    {
        delayCoroutine = StartCoroutine(DelayChangeStateTime(state, time));
    }

    private IEnumerator DelayChangeStateTime(GamePlayState state, float time)
    {
        yield return new WaitForSeconds(time);

        ChangeState(state);
    }

    public void OnTouch()
    {
        OnPlayerTouchScreen?.Invoke();
    }
}
