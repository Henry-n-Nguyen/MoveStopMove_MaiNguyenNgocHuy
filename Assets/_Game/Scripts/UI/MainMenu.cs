using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;
using TMPro;

public class MainMenu : UICanvas
{
    private const string ANIM_INTRO = "intro";
    private const string ANIM_OUTRO = "outro";

    [Header("Anim")]
    [SerializeField] private Animator anim;

    [Space(0.3f)]
    [Header("References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Image levelIcon;

    [Space(0.3f)]
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

        coinText.text = data.coin.ToString();

        highScoreText.text = "Zone : " + data.currentHighestLevel.ToString() + "\nBest : #" + data.currentHighestRank.ToString();

        levelIcon.sprite = LevelManager.Ins.GetLevelIcon(data.currentLevel);

        IsVibrationOn(data.isVibrationOn);
        IsSoundOn(data.isSoundOn);
    }

    public override void Open()
    {
        base.Open();

        ChangeAnim(ANIM_INTRO);
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

    public void PlayGame()
    {
        LevelManager.Ins.OnPlay();
    }

    public void EnterWeaponShop()
    {
        LevelManager.Ins.OnWeaponShop();
    }

    public void EnterCostumeShop()
    {
        LevelManager.Ins.OnCostumeShop();
    }

    public override void Close(float time)
    {
        ChangeAnim(ANIM_OUTRO);
    }

    private void ChangeAnim(string animName)
    {
        anim.ResetTrigger(animName);
        anim.SetTrigger(animName);
    }
}
