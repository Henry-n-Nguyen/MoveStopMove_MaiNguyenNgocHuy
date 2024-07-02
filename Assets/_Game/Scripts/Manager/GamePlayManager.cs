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

        ChangeState(GamePlayState.MainMenu);
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

    // States
    private void MainMenuState()
    {
        UIManager.instance.CloseAll();

        UIManager.instance.OpenUI<MainMenu>();
    }

    private void WinState()
    {
        UIManager.instance.CloseDirectly<DynamicJoyStick>();

        coinToEarn = player.point * COIN_CONVERT_RATE + COIN_BONUS;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        triggerWin = StartCoroutine(TriggerWinStateAfterTime(2.5f));
    }
    
    private void LoseState()
    {
        UIManager.instance.CloseDirectly<DynamicJoyStick>();

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

    private void WeaponShopState()
    {
        UIManager.instance.CloseAll();

        UIManager.instance.OpenUI<WeaponShop>();
    }

    private void CostumeShopState()
    {
        UIManager.instance.CloseAll();

        UIManager.instance.OpenUI<CostumeShop>();
    }

    private void AwardState()
    {
        UIManager.instance.CloseAll();

        UIManager.instance.OpenUI<Award>();
    }

    private void SettingsState()
    {
        UIManager.instance.OpenUI<Settings>();
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

    public void EarnCoin()
    {
        coinToEarn = player.point * COIN_CONVERT_RATE;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();
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

        switch (currentGamePlayState)
        {
            case GamePlayState.MainMenu: MainMenuState(); break;
            case GamePlayState.Lose: LoseState(); break;
            case GamePlayState.Win: WinState(); break;
            case GamePlayState.CostumeShop: CostumeShopState(); break;
            case GamePlayState.WeaponShop: WeaponShopState(); break;
            case GamePlayState.Award: AwardState(); break;
            case GamePlayState.Settings: SettingsState(); break;
        }

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
