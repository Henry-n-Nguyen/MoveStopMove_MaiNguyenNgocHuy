using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

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
        OnLoading();
    }

    private void OnLoading()
    {
        currentGamePlayState = GamePlayState.None;

        StartCoroutine(LoadingGame(5f));
    }

    public void OnInit()
    {
        aliveCharacterAmount = characterAmount;
        data = UserDataManager.instance.userData;

        coinToEarn = 0;
    }

    private IEnumerator LoadingGame(float time)
    {
        UIManager.instance.OpenUI<Loading>();

        BotGenerator.instance.SpawnPlayer();

        yield return new WaitForSeconds(time);

        UIManager.instance.CloseDirectly<Loading>();
        UIManager.instance.OpenUI<MainMenu>();
    }

    public void WinGame()
    {
        coinToEarn = player.point * 10;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(WinGame(2.5f));
    }

    private IEnumerator WinGame(float time)
    {
        yield return new WaitForSeconds(time);

        currentGamePlayState = GamePlayState.Win;

        player.OnInit();
        player.Win();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Win>();
    }

    public void LoseGame()
    {
        coinToEarn = player.point * 10;
        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(LoseGame(2.5f));
    }

    private IEnumerator LoseGame(float time)
    {
        yield return new WaitForSeconds(time);

        currentGamePlayState = GamePlayState.Lose;

        player.OnInit();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }
}
