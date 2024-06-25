using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class BotGenerator : MonoBehaviour
{
    public static BotGenerator instance;

    [Header("Pre-Setup")]
    [SerializeField] private Player playerPrefab;

    [Header("References")]
    [SerializeField] private Transform characterHolder;

    [SerializeField] private LayerMask spawnPointLayer;

    [Header("EquipmentDataSO")]
    [SerializeField] private EquipmentDataSO equipmentDataSO;

    [Header("NameDataSO")]
    [SerializeField] private NameDataSO nameDataSO;

    // Private variables
    private int index = 1;

    private List<int> queueToSpawnBot = new List<int>();
    private bool isSpawning = false;

    public int characterInQueueToSpawn;

    public int characterInBattleAmount = 1;

    private List<Transform> activatedTransform = new List<Transform>();
    private List<Transform> inActivatedTransform = new List<Transform>();

    // Coroutines
    private Coroutine deActiveTransformCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SubscribeEvent();
        OnInit();
    }

    private void SubscribeEvent()
    {
        GamePlayManager.instance.OnAliveCharacterAmountChanged += ReduceCharacterInBattle;
    }

    public void OnInit()
    {
        activatedTransform.Clear();
        inActivatedTransform.Clear();
        queueToSpawnBot.Clear();

        isSpawning = false;

        characterInBattleAmount = 1;
    }

    public void AddSpawnQueue(int quantity)
    {
        queueToSpawnBot.Add(quantity);

        if (!isSpawning)
        {
            SpawnBots();
        } 
    }

    public void SpawnPlayer()
    {
        if (GamePlayManager.instance.player != null)
        {
            GamePlayManager.instance.player.OnInit();
            BotPool.Collect();
        }
        else
        {
            Player createdPlayer = Instantiate(playerPrefab, characterHolder);

            createdPlayer.LoadDataFromUserData();

            GamePlayManager.instance.player = createdPlayer;

            CameraFollow.instance.target = GamePlayManager.instance.player;
            CameraFollow.instance.OnInit();
        }
    }

    public void SpawnBots()
    {
        isSpawning = true;

        inActivatedTransform.Clear();
        inActivatedTransform = FindNearbySpawnPoints(GamePlayManager.instance.player.characterTransform);

        if (inActivatedTransform.Count == 0)
        {
            return;
        }

        int quantity = queueToSpawnBot[0];
        queueToSpawnBot.RemoveAt(0);

        int randomNumber = 0;

        //for (int i = 0; i < quantity; i++)
        for (int i = 0; i < quantity; i++)
        {
            if (characterInBattleAmount >= GamePlayManager.instance.aliveCharacterAmount || characterInBattleAmount > 8)
            {
                continue;
            }

            // Pick randomly spawnPoint to spawn Bots
            randomNumber = Random.Range(0, inActivatedTransform.Count);

            Transform targetTransform = inActivatedTransform[randomNumber];
            inActivatedTransform.Remove(targetTransform);

            activatedTransform.Add(targetTransform);
            deActiveTransformCoroutine = StartCoroutine(DeActiveTargetTransformAfterTime(targetTransform, 10f));

            Vector3 randomPosition = targetTransform.position;

            SpawnBot(randomPosition);
        }

        if (queueToSpawnBot.Count > 0) SpawnBots();
    }

    private void SpawnBot(Vector3 pos)
    {
        int randomNumber = 0;

        // Spawn Bot
        Enemy character = BotPool.Spawn<Enemy>(pos, Quaternion.identity);
        if (character == null) Debug.Log("Char null");
        character.index = index;
        index++;

        character.OnInit();

        int playerPoint = GamePlayManager.instance.player.point;
        character.point = Random.Range(playerPoint, playerPoint + 4);
        character.OnPointChanges();

        character.ChangeName(nameDataSO.GetRandomName());

        List<Weapon> weaponList = equipmentDataSO.GetWeaponList();
        randomNumber = Random.Range(0, weaponList.Count);
        character.Equip(EquipmentType.Weapon, weaponList[randomNumber]);

        List<Hat> hatList = equipmentDataSO.GetHatList();
        randomNumber = Random.Range(0, hatList.Count);
        character.Equip(EquipmentType.Hat, hatList[randomNumber]);

        List<Pant> pantList = equipmentDataSO.GetPantList();
        randomNumber = Random.Range(0, pantList.Count);
        character.Equip(EquipmentType.Pant, pantList[randomNumber]);

        List<Skin> skinList = equipmentDataSO.GetSkinList();
        randomNumber = Random.Range(0, skinList.Count);
        character.Equip(EquipmentType.Skin, skinList[randomNumber]);

        characterInBattleAmount++;
        isSpawning = false;
    }

    private IEnumerator DeActiveTargetTransformAfterTime(Transform target, float time)
    {
        yield return new WaitForSeconds(time);
        activatedTransform.Remove(target);
        inActivatedTransform.Add(target);
    }

    private List<Transform> FindNearbySpawnPoints(Transform targetTransform)
    {
        List<Transform> nearbySpawnPointsTransform = new List<Transform>();

        float distanceToDetectTargetObject = 65f;
        Collider[] spawnPointCollides = Physics.OverlapSphere(targetTransform.position, distanceToDetectTargetObject, spawnPointLayer);

        if (spawnPointCollides.Length > 0)
        {
            for (int i = 0; i < spawnPointCollides.Length; i++)
            {
                Transform spawnPoint = Cache.GetSpawnpoint(spawnPointCollides[i]);
                nearbySpawnPointsTransform.Add(spawnPoint);
            }
        }

        if (nearbySpawnPointsTransform.Count > 0)
        {
            Transform nearestSpawnpoint = nearbySpawnPointsTransform[0];

            float currentDistance = Vector3.Distance(targetTransform.position, nearestSpawnpoint.position);

            for (int i = 0; i < nearbySpawnPointsTransform.Count; i++)
            {
                float newDistance = Vector3.Distance(targetTransform.position, nearbySpawnPointsTransform[i].position);

                if (newDistance < currentDistance)
                {
                    currentDistance = newDistance;
                    nearestSpawnpoint = nearbySpawnPointsTransform[i];
                }
            }

            nearbySpawnPointsTransform.Remove(nearestSpawnpoint);
        }

        return nearbySpawnPointsTransform;
    }

    private void ReduceCharacterInBattle()
    {
        characterInBattleAmount--;
        characterInQueueToSpawn++;
    }
}
