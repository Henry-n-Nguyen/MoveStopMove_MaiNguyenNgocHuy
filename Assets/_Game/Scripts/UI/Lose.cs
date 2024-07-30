using HuySpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lose : UICanvas
{
    [SerializeField] private TextMeshProUGUI earnCoinText;
    [SerializeField] private TextMeshProUGUI rankText;

    private UserData data;

    private void Awake()
    {
        data = UserDataManager.Ins.userData;
    }

    public override void Setup()
    {
        base.Setup();

        earnCoinText.text = "+ " + (CharacterManager.Ins.player.point).ToString();

        CharacterManager.Ins.OnInit();

        int rank = CharacterManager.Ins.CharacterAlive;
        rankText.text = "#" + rank.ToString();

        if (rank < data.currentHighestRank)
        {
            data.currentHighestLevel = UserDataManager.Ins.userData.currentLevel + 1;
            data.currentHighestRank = rank;
            UserDataManager.Ins.SaveData();
        }
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
