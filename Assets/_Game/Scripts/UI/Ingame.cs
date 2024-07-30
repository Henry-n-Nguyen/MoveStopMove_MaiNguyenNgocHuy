using HuySpace;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ingame : UICanvas
{
    [Header("Text references")]
    [SerializeField] private TextMeshProUGUI aliveCharacterText;
    [SerializeField] private GameObject helpToMovePart;

    [Space(0.3f)]
    [Header("Joystick")]
    [SerializeField] private DynamicJoyStick joyStick;

    // Coroutines
    private Coroutine spawnBoosterCoroutine;

    private void Start()
    {
        SubcribeEventAction();
    }

    private void SubcribeEventAction()
    {
        joyStick.OnTouchScreenAction += NonDisplayHelpToMove;
        joyStick.OnReleaseScreenAction += DisplayHelpToMove;
        CharacterManager.Ins.OnBotDieAction += UpdateCounter;
    }

    public override void Setup()
    {
        base.Setup();

        helpToMovePart.SetActive(true);

        aliveCharacterText.text = "Alive : " + CharacterManager.Ins.CharacterAlive.ToString();

        spawnBoosterCoroutine = StartCoroutine(SpawnBoosterAfterTime(10f));

        joyStick.gameObject.SetActive(true);
    }

    public override void CloseDirectly()
    {
        if (spawnBoosterCoroutine != null) StopCoroutine(spawnBoosterCoroutine);

        base.CloseDirectly();
    }

    private IEnumerator SpawnBoosterAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Vector3 pos = LevelManager.Ins.GetRandomPos();
        SpawnRandomBooster(pos);
        spawnBoosterCoroutine = StartCoroutine(SpawnBoosterAfterTime(10f));
    }

    private void SpawnRandomBooster(Vector3 pos)
    {
        BoostType[] boosterTypes = (BoostType[])Enum.GetValues(typeof(BoostType));
        int randomIndex = UnityEngine.Random.Range(1, boosterTypes.Length);
        BoostType randomBoosterType = (BoostType)boosterTypes.GetValue(randomIndex);

        SimplePool.Spawn<AbstractBooster>((PoolType)randomBoosterType, pos, Quaternion.identity);
    }

    public void UpdateCounter()
    {
        aliveCharacterText.text = "Alive : " + CharacterManager.Ins.CharacterAlive.ToString();
        if (CharacterManager.Ins.CharacterAlive == 1) LevelManager.Ins.OnWin();
    }

    public void DisplayHelpToMove()
    {
        if (CharacterManager.Ins.player.IsDead) return;

        helpToMovePart.SetActive(true);
    }

    public void NonDisplayHelpToMove()
    {
        helpToMovePart.SetActive(false);
    }

    public void OpenSettings()
    {
        if (spawnBoosterCoroutine != null) StopCoroutine(spawnBoosterCoroutine);
        joyStick.gameObject.SetActive(false);
        LevelManager.Ins.OnSettings();
    }
}
