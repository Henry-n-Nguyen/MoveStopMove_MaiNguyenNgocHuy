using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;
using TMPro;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Image levelIcon;

    [SerializeField] private GameObject vibrationOn;
    [SerializeField] private GameObject vibrationOff;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;

    private UserData data;

    private void Awake()
    {
        data = UserDataManager.instance.userData;
    }

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        StartCoroutine(Loading(3f));

        coinText.text = data.coin.ToString();

        highScoreText.text = "Zone : " + data.currentHighestLevel.ToString() + "\nBest : #" + data.currentHighestRank.ToString();

        levelIcon.sprite = LevelManager.instance.GetLevelIcon(data.currentLevel);

        LevelManager.instance.SpawnMap();

        GamePlayManager.instance.OnInit();
        CameraManager.instance.TurnOnCamera(CameraState.MainMenu);

        BotGenerator.instance.OnInit();
        BotGenerator.instance.SpawnPlayer();
        BotGenerator.instance.AddSpawnQueue(GamePlayManager.instance.startCharacterAmount);

        VibrationOnOff();
        SoundOnOff();
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

    public void ChangeVibrationState()
    {
        data.isVibrationOn = !data.isVibrationOn;
        VibrationOnOff();
    }

    public void VibrationOnOff()
    {
        if (data.isVibrationOn)
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
        }
        else
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
        }
    }

    public void ChangeSoundState()
    {
        data.isSoundOn = !data.isSoundOn;
        SoundOnOff();
    }

    public void SoundOnOff()
    {
        if (data.isSoundOn)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

    private IEnumerator Loading(float time)
    {
        UIManager.instance.OpenUI<Loading>();

        yield return new WaitForSeconds(time);

        UIManager.instance.CloseDirectly<Loading>();
    }
}
