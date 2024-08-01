using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HuySpace;

public class Award : UICanvas
{
    [SerializeField] private TextMeshProUGUI earnCoinText;

    public override void Setup()
    {
        base.Setup();

        earnCoinText.text = "+ " + (LevelManager.Ins.coinToEarn).ToString();
    }

    public void ReturnHome()
    {
        LevelManager.Ins.OnMainMenu();
    }
}
