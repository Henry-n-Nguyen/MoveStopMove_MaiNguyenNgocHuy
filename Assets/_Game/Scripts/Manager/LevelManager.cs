using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using HuySpace;
using System;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    // inspector
    [SerializeField] private List<Sprite> levelIcons = new List<Sprite>();
    [SerializeField] private List<Map> mapPrefabs = new List<Map>();
    [SerializeField] private Transform mapHolder;
    [Space(0.3f)]
    [SerializeField] private Map currentMap = null;

    // Editor
    [field :SerializeField] public int startCharacterAmount { get; private set; }
    [field :SerializeField] public int currentMapCharacterAmount { get; private set; }
    [field: SerializeField] public int currentLevel { get; private set; } = 0;

    public int HigestLevel => mapPrefabs.Count;

    private bool isFirstStart = true;
    private UserData data;

    //Function
    private void Start()
    {
        OnInit();
    }

    // State Function
    public void OnInit()
    {
        data = UserDataManager.Ins.userData;

        SpawnMap(data.currentLevel);
        CharacterManager.Ins.SpawnPlayer(Vector3.zero);
        CharacterManager.Ins.SetAmountOfCharacterAlive(currentMapCharacterAmount);
        CharacterManager.Ins.SpawnBotWithQty(startCharacterAmount);

        UIManager.Ins.OpenUI<WeaponShop>();
        UIManager.Ins.OpenUI<CostumeShop>();

        OnMainMenu();
    }

    public void OnMainMenu()
    {
        SimplePool.CollectAll();
        CharacterManager.Ins.OnInit();
        CharacterManager.Ins.SpawnBotWithQty(startCharacterAmount);

        CameraManager.Ins.ChangeCameraState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();

        GamePlayManager.Ins.ChangeState(GameState.MainMenu);
    }

    public void OnPlay()
    {
        CameraManager.Ins.ChangeCameraState(CameraState.Ingame);

        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Ingame>();

        GamePlayManager.Ins.ChangeState(GameState.Ingame);
    }

    public void OnSettings()
    {
        UIManager.Ins.OpenUI<Settings>();

        GamePlayManager.Ins.ChangeState(GameState.Settings);
    }

    public void OnRevive()
    {
        CameraManager.Ins.ChangeCameraState(CameraState.Revive);

        UIManager.Ins.OpenUI<Revive>();
    }

    public void OnLose()
    {
        CameraManager.Ins.ChangeCameraState(CameraState.Lose);

        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Lose>();
    }

    public void OnWin()
    {
        CameraManager.Ins.ChangeCameraState(CameraState.Win);

        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Win>();

        GamePlayManager.Ins.ChangeState(GameState.Ingame);
    }

    public void OnWeaponShop()
    {
        GamePlayManager.Ins.ChangeState(GameState.WeaponShop);

        UIManager.Ins.CloseUI<MainMenu>(0);
        UIManager.Ins.OpenUI<WeaponShop>();
    }

    public void OnCostumeShop()
    {
        CameraManager.Ins.ChangeCameraState(CameraState.CostumeShop);

        UIManager.Ins.CloseUI<MainMenu>(0);
        UIManager.Ins.OpenUI<CostumeShop>();

        GamePlayManager.Ins.ChangeState(GameState.CostumeShop);
    }

    public void OnAward()
    {
        UIManager.Ins.OpenUI<Award>();
    }

    public void OnDespawn()
    {
        
    }

    // Spawn/Despawn Map
    public void SpawnMap(int level)
    {
        if (currentMap != null) DespawnMap();
        currentMap = Instantiate(mapPrefabs[level], mapHolder);
        currentMapCharacterAmount = currentMap.capacity;
        startCharacterAmount = currentMap.startCharacterAmount;
    }
    public void DespawnMap()
    {
        if (currentMap == null) return;
        Destroy(currentMap);
    }

    // Get Random Position from map
    public Vector3 GetRandomPos()
    {
        float width = currentMap.UsebleWidth;
        float length = currentMap.UsableLength;
        float height = currentMap.UsableHeight;

        Vector3 pos = Vector3.right * Random.Range(-width, width)
            + Vector3.up * (height - 1.5f) + Vector3.forward * Random.Range(-length, length);

        return pos;
    }

    public Vector3 GetRandomPosAroundTarget(Transform targetTransform, float radius)
    {
        // create random point in a walkRadius
        Vector3 randomDirection = Random.insideUnitSphere * radius;

        // reference point was created to current tranform and find random point with it
        randomDirection += targetTransform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1);

        // return result
        return hit.position;
    }

    // Getter
    public Sprite GetLevelIcon(int level)
    {
        return levelIcons[level];
    }
}
