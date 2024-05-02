using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System.Linq;

public class BotGenerator : MonoBehaviour
{
    public static BotGenerator instance;

    [Header("Button to Spawn :V")]
    [SerializeField] private bool isSpawn;

    [Header("Pre-Setup")]
    [SerializeField] private Transform holder;

    [SerializeField] private AbstractCharacter prefab;

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [SerializeField] private LayerMask spawnPointLayer;

    // Private variables
    private int index = 1;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        if (isSpawn)
        {
            SpawnBot(8);
            isSpawn = false;
        }
    }

    public void SpawnBot(int quantity)
    {
        List<Transform> spawnPointList = FindNearbySpawnPoints(playerTransform);

        List<Transform> activatedTransform = new List<Transform>();

        int randomNumber = 0;

        for (int i = 0; i < quantity; i++)
        {
            // Pick randomly spawnPoint to spawn Bots
            randomNumber = Random.Range(0, spawnPointList.Count);

            while (activatedTransform.Count > 0 && activatedTransform.IndexOf(spawnPointList[randomNumber]) != -1)
            {
                randomNumber = Random.Range(0, spawnPointList.Count);
            }

            activatedTransform.Add(spawnPointList[randomNumber]);

            Vector3 randomPosition = spawnPointList[randomNumber].position;

            // Spawn Bot
            AbstractCharacter character = BotPool.Spawn<Enemy>(randomPosition, Quaternion.identity);

            character.index = index;
            index++;

            randomNumber = Random.Range(0, EquipmentManager.instance.weaponList.Count);
            character.Equip(EquipmentType.Weapon, EquipmentManager.instance.weaponList[randomNumber]);

            randomNumber = Random.Range(0, EquipmentManager.instance.hatList.Count);
            character.Equip(EquipmentType.Hat, EquipmentManager.instance.hatList[randomNumber]);

            randomNumber = Random.Range(0, MaterialManager.instance.pantMaterialList.Count);
            character.Equip(EquipmentType.Pant, MaterialManager.instance.pantMaterialList[randomNumber]);

            randomNumber = Random.Range(0, MaterialManager.instance.skinMaterialList.Count);
            character.Equip(EquipmentType.Skin, MaterialManager.instance.skinMaterialList[randomNumber]);
        }
    }

    public List<Transform> FindNearbySpawnPoints(Transform targetTransform)
    {
        List<Transform> nearbySpawnPoints = new List<Transform>();

        Collider[] spawnPointsInRange = Physics.OverlapSphere(targetTransform.position, 90f, spawnPointLayer);

        if (spawnPointsInRange.Length > 0)
        {
            foreach (Collider spawnPoint in spawnPointsInRange)
            {
                nearbySpawnPoints.Add(spawnPoint.transform);
            }
        }

        Transform nearestSpawnpoint = nearbySpawnPoints[0];

        float currentDistance = Vector3.Distance(targetTransform.position, nearestSpawnpoint.position);

        foreach (Transform spawnPoint in nearbySpawnPoints)
        {
            float newDistance = Vector3.Distance(targetTransform.position, spawnPoint.position);

            if (newDistance < currentDistance)
            {
                currentDistance = newDistance;
                nearestSpawnpoint = spawnPoint;
            }
        }

        nearbySpawnPoints.Remove(nearestSpawnpoint);

        return nearbySpawnPoints;
    }
}
