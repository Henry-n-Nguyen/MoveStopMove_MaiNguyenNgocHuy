using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    [HideInInspector] public GamePlayState currentGamePlayState;

    [HideInInspector] public Player player;

    [SerializeField] private int characterAmount;

    [HideInInspector] public int aliveCharacterAmount;

    [HideInInspector] public int coinToEarn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnInit();
        OnLoading();
    }

    private void Update()
    {
        if (aliveCharacterAmount == 1 && !player.IsDead)
        {
            WinGame();
        }
    }

    private void OnLoading()
    {
        currentGamePlayState = GamePlayState.None;

        StartCoroutine(LoadingGame(5f));
    }

    public void OnInit()
    {
        aliveCharacterAmount = characterAmount;
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
        UserData data = UserDataManager.instance.userData;

        currentGamePlayState = GamePlayState.Win;

        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(WinGame(2f));
    }

    private IEnumerator WinGame(float time)
    {
        yield return new WaitForSeconds(time);

        player.OnInit();
        player.Win();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Win>();
    }

    public void LoseGame()
    {
        UserData data = UserDataManager.instance.userData;

        currentGamePlayState = GamePlayState.Lose;

        data.coin += coinToEarn;
        UserDataManager.instance.Save();

        StartCoroutine(LoseGame(2f));
    }

    private IEnumerator LoseGame(float time)
    {
        yield return new WaitForSeconds(time);
        
        player.OnInit();

        UIManager.instance.CloseAll();
        UIManager.instance.OpenUI<Lose>();
    }
}
