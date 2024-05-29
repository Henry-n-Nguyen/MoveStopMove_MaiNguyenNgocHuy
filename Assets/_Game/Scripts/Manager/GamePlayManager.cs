using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    [SerializeField] private int characterAmount;

    [HideInInspector] public GamePlayState currentGamePlayState;

    [HideInInspector] public Player player;

    [HideInInspector] public Transform playerTransform;

    [HideInInspector] public int aliveCharacterAmount;

    [HideInInspector] public int coinToEarn;

    private UserData data;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        data = UserDataManager.instance.userData;

        ChangeState(GamePlayState.None);

        aliveCharacterAmount = characterAmount;

        coinToEarn = 0;

        UIManager.instance.OpenUI<MainMenu>();
    }

    public void WinGame()
    {
        coinToEarn = player.point * 12;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(WinGame(2.5f));
    }

    private IEnumerator WinGame(float time)
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
        coinToEarn = player.point * 12;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(LoseGame(2.5f));
    }

    private IEnumerator LoseGame(float time)
    {
        yield return new WaitForSeconds(time);

        player.OnInit();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }

    public event Action OnAliveCharacterAmountChanged;

    public void CharacterDied()
    {
        aliveCharacterAmount--;

        OnAliveCharacterAmountChanged?.Invoke();
    }

    public event Action OnUIChanged;

    public void ChangeState(GamePlayState state)
    {
        currentGamePlayState = state;

        OnUIChanged?.Invoke();
    }
}
