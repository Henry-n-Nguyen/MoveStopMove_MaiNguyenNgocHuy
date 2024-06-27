using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
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
        Time.timeScale = 0.0f;

        GamePlayManager.instance.ChangeState(GamePlayState.None);

        VibrationOnOff();
        SoundOnOff();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;

        GamePlayManager.instance.ChangeState(GamePlayState.Ingame);

        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
    }

    public void ReturnHome()
    {
        Time.timeScale = 1.0f;

        UIManager.instance.CloseDirectly<Settings>();
        UIManager.instance.CloseDirectly<Ingame>();

        UIManager.instance.OpenUI<MainMenu>();
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
}
