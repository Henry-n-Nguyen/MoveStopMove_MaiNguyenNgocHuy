using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;
using TMPro;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Image levelIcon;

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        StartCoroutine(Loading(3f));

        coinText.text = UserDataManager.instance.userData.coin.ToString();

        levelIcon.sprite = LevelManager.instance.GetLevelIcon(UserDataManager.instance.userData.currentLevel);

        LevelManager.instance.SpawnMap();

        GamePlayManager.instance.OnInit();
        CameraManager.instance.TurnOnCamera(CameraState.MainMenu);

        BotGenerator.instance.SpawnPlayer();
        BotGenerator.instance.AddSpawnQueue(GamePlayManager.instance.startCharacterAmount);
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
