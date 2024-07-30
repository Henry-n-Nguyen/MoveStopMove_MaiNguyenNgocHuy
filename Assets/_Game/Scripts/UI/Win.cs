using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Win : UICanvas
{
    [SerializeField] private TextMeshProUGUI earnCoinText;

    private UserData data;

    private void Awake()
    {
        data = UserDataManager.Ins.userData;
    }

    public override void Setup()
    {
        base.Setup();

        earnCoinText.text = "+ " + (CharacterManager.Ins.player.point).ToString();

        CharacterManager.Ins.player.OnInit();

        int currentLevel = UserDataManager.Ins.userData.currentLevel;
        int highestLevel = LevelManager.Ins.HigestLevel;

        UserDataManager.Ins.userData.currentHighestRank = 1;
        UserDataManager.Ins.userData.currentHighestLevel = UserDataManager.Ins.userData.currentLevel + 1;
        UserDataManager.Ins.userData.currentLevel = currentLevel + 1 > highestLevel ? highestLevel : currentLevel + 1;
        UserDataManager.Ins.SaveData();
    }

    public void ReturnHome()
    {
        LevelManager.Ins.OnMainMenu();
    }

    public void TripleAward()
    {
        LevelManager.Ins.OnAward();
    }
}
