using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using TMPro;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        StartCoroutine(Loading(3f));

        coinText.text = UserDataManager.instance.userData.coin.ToString();

        GamePlayManager.instance.OnInit();
        CameraManager.instance.TurnOnCamera(CameraState.MainMenu);

        BotGenerator.instance.SpawnPlayer();
        BotGenerator.instance.SpawnBots(8);
    }

    public void PlayGame()
    {
        UIManager.instance.CloseDirectly<MainMenu>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
        UIManager.instance.OpenUI<Ingame>();
    }

    public void EnterWeaponShop()
    {
        UIManager.instance.CloseDirectly<MainMenu>();

        UIManager.instance.OpenUI<WeaponShop>();
    }

    public void EnterCostumeShop()
    {
        UIManager.instance.CloseDirectly<MainMenu>();

        UIManager.instance.OpenUI<CostumeShop>();
    }

    private IEnumerator Loading(float time)
    {
        UIManager.instance.OpenUI<Loading>();

        yield return new WaitForSeconds(time);

        UIManager.instance.CloseDirectly<Loading>();
    }
}
