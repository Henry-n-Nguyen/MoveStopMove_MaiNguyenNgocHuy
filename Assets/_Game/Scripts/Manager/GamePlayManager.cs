using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    public bool onPause;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnHold();
        OnInit();
    }

    public void OnInit()
    {
        StartCoroutine(LoadingGame());   
    }

    private IEnumerator LoadingGame()
    {
        UIManager.instance.OpenUI<Loading>();

        BotGenerator.instance.SpawnBot(8);

        yield return new WaitForSeconds(5f);

        UIManager.instance.CloseDirectly<Loading>();
        UIManager.instance.OpenUI<MainMenu>();
    }

    public void OnHold()
    {
        onPause = true;
    }

    public void ResumeGame()
    {
        onPause = false;
    }
}
