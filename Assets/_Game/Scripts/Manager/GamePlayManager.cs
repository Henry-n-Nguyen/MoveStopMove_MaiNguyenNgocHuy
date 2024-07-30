using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System;
using UnityEditor;

public class GamePlayManager : Singleton<GamePlayManager>
{
    private static GameState gameState;

    private Coroutine delayCoroutine;

    public event Action OnUIChangedAction;

    public void ChangeState(GameState state)
    {
        gameState = state;

        OnUIChangedAction?.Invoke();
    }

    public void ChangeStateAfterTime(GameState state, float time)
    {
        delayCoroutine = StartCoroutine(DelayChangeState(state, time));
    }

    private IEnumerator DelayChangeState(GameState state, float time)
    {
        yield return new WaitForSeconds(time);

        ChangeState(state);
    }

    public void StopDelayChangeState()
    {
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
    }

    public static bool IsState(GameState state) => gameState == state;

    private void Awake()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;

        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;

        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    private void Start()
    {
        //UIManager.Ins.OpenUI<MainMenu>();
    }
}
