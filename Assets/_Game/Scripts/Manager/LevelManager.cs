using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private List<Sprite> levelIcons = new List<Sprite>();
    [SerializeField] private List<GameObject> mapPrefabs = new List<GameObject>();
    [SerializeField] private Transform mapHolder;

    private UserData data;

    public int currentLevel = -1;
    private GameObject currentMap = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        data = UserDataManager.instance.userData;
    }

    public void SpawnMap()
    {
        if (currentLevel == data.currentLevel) return;

        if (currentMap != null) DespawnMap();

        GameObject createdMap = Instantiate(mapPrefabs[data.currentLevel], mapHolder);
        currentMap = createdMap;

        currentLevel = data.currentLevel;
    }

    public void DespawnMap()
    {
        Destroy(currentMap);
    }

    public Sprite GetLevelIcon(int level)
    {
        return levelIcons[level];
    }

    public int GetCurentMaxLevel()
    {
        return mapPrefabs.Count;
    }
}
