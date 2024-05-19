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

    [HideInInspector] public Player player = null;

    // Private variables
    private int index = 1;

    public int characterInBattleAmount = 1;

    private List<Transform> activatedTransform = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    public void SpawnPlayer()
    {
        if (player != null)
        {
            player.OnInit();
        }
        else
        {
            Player createdPlayer = Instantiate(playerPrefab, characterHolder);

            player = createdPlayer;

            GamePlayManager.instance.player = player;
            CameraFollow.instance.target = player.transform;
            UserDataManager.instance.player = player;
        }
    }

    public void SpawnBot(int quantity)
    {
        List<Transform> spawnPointList = FindNearbySpawnPoints(player.transform);

        int randomNumber = 0;

        for (int i = 0; i < quantity; i++)
        {
            if (activatedTransform.Count == spawnPointList.Count || characterInBattleAmount >= GamePlayManager.instance.aliveCharacterAmount) continue;

            // Pick randomly spawnPoint to spawn Bots
            randomNumber = Random.Range(0, spawnPointList.Count);

            while (activatedTransform.Count > 0 && activatedTransform.IndexOf(spawnPointList[randomNumber]) != -1)
            {
                randomNumber = Random.Range(0, spawnPointList.Count);
            }

            activatedTransform.Add(spawnPointList[randomNumber]);
            StartCoroutine(RemoveActivatedTransform(spawnPointList[randomNumber], 5f));

            Vector3 randomPosition = spawnPointList[randomNumber].position;

            // Spawn Bot
            Enemy character = BotPool.Spawn<Enemy>(randomPosition, Quaternion.identity);

            character.index = index;
            index++;

            character.point = Random.Range(player.point, player.point + 4);

            character.OnInit();

            List<Weapon> weaponList = EquipmentManager.instance.GetWeaponList();
            randomNumber = Random.Range(0, weaponList.Count);
            character.Equip(EquipmentType.Weapon, weaponList[randomNumber]);

            List<Hat> hatList = EquipmentManager.instance.GetHatList();
            randomNumber = Random.Range(0, hatList.Count);
            character.Equip(EquipmentType.Hat, hatList[randomNumber]);

            List<Pant> pantList = MaterialManager.instance.GetPantList();
            randomNumber = Random.Range(0, pantList.Count);
            character.Equip(EquipmentType.Pant, pantList[randomNumber]);

            List<Skin> skinList = MaterialManager.instance.GetSkinList();
            randomNumber = Random.Range(0, skinList.Count);
            character.Equip(EquipmentType.Skin, skinList[randomNumber]);

            characterInBattleAmount++;
        }
    }

    private IEnumerator RemoveActivatedTransform(Transform target, float time)
    {
        yield return new WaitForSeconds(time);
        activatedTransform.Remove(target);
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
