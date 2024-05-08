using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterGenerator : MonoBehaviour
{
    public static BoosterGenerator instance;

    [Header("Button to Spawn :V")]
    [SerializeField] private bool isSpawn;

    [Header("Pre-Setup")]
    [SerializeField] private List<AbstractBooster> prefabs;

    [SerializeField] private Transform holder;

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [SerializeField] private LayerMask spawnPointLayer;

    public List<Transform> activatedTransform = new List<Transform>();

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
            SpawnBooster(1);
            isSpawn = false;
        }
    }

    public void SpawnBooster(int quantity)
    {
        List<Transform> spawnPointList = FindNearbySpawnPoints(playerTransform);

        if (spawnPointList.Count > quantity)
        {
            int randomNumber = 0;

            for (int i = 0; i < quantity; i++)
            {
                // Pick random position that not in activated position list
                randomNumber = Random.Range(0, spawnPointList.Count);

                while (activatedTransform.Count > 0 && activatedTransform.IndexOf(spawnPointList[randomNumber]) != -1)
                {
                    randomNumber = Random.Range(0, spawnPointList.Count);
                }

                activatedTransform.Add(spawnPointList[randomNumber]);
                StartCoroutine(RemoveActivatedTransform(spawnPointList[randomNumber], 15f));

                Vector3 randomPosition = spawnPointList[randomNumber].position;

                // Spawn Booster
                randomNumber = Random.Range(0, prefabs.Count);

                AbstractBooster booster = Instantiate(prefabs[randomNumber], randomPosition, Quaternion.identity, holder);

                booster.Spawn();
            }
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

        Collider[] spawnPointsInRange = Physics.OverlapSphere(targetTransform.position, 50f, spawnPointLayer);

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