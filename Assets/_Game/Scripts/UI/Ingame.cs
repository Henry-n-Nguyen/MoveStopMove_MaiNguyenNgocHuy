using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame : UICanvas
{
    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        CameraManager.instance.TurnOnCamera(CameraState.MainCamera);
        StartCoroutine(SpawnBoosterAfterTime(15f));
    }

    private IEnumerator SpawnBoosterAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        BoosterGenerator.instance.SpawnBooster(2);
    }

    public void OpenSettings()
    {
        UIManager.instance.CloseDirectly<DynamicJoyStick>();
        UIManager.instance.CloseDirectly<Ingame>();

        UIManager.instance.OpenUI<Settings>();
    }
}
