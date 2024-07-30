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
        data = UserDataManager.Ins.userData;
    }

    public override void Setup()
    {
        base.Setup();

        IsVibrationOn(data.isVibrationOn);
        IsSoundOn(data.isSoundOn);
    }

    public void IsVibrationOn(bool status)
    {
        data.isVibrationOn = status;

        vibrationOn.SetActive(status);
        vibrationOff.SetActive(!status);
    }

    public void ChangeVibrationState()
    {
        IsVibrationOn(!data.isVibrationOn);
    }

    public void IsSoundOn(bool status)
    {
        data.isSoundOn = status;

        soundOn.SetActive(status);
        soundOff.SetActive(!status);
    }

    public void ChangeSoundState()
    {
        IsSoundOn(!data.isSoundOn);
    }

    public void ResumeGame()
    {
        LevelManager.Ins.OnPlay();
    }

    public void ReturnHome()
    {
        LevelManager.Ins.OnMainMenu();
    }
}
