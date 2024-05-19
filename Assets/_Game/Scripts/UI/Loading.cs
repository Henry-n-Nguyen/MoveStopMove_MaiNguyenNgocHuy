using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : UICanvas
{
    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        GamePlayManager.instance.currentGamePlayState = GamePlayState.Loading;
    }
}
