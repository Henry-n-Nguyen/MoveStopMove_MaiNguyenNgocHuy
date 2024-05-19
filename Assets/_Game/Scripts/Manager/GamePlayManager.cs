using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    public GamePlayState currentGamePlayState;

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
        currentGamePlayState = GamePlayState.None;

        StartCoroutine(LoadingGame());   
    }

    private IEnumerator LoadingGame()
    {
        UIManager.instance.OpenUI<Loading>();

        BotGenerator.instance.SpawnPlayer();

        yield return new WaitForSeconds(5f);

        UIManager.instance.CloseDirectly<Loading>();
        UIManager.instance.OpenUI<MainMenu>();
    }
}
