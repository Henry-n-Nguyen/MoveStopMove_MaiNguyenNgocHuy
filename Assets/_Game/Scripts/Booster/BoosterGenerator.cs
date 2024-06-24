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
    [SerializeField] private AbstractCharacter player;

    [SerializeField] private LayerMask spawnPointLayer;

    private AbstractBooster createdBooster;

    public List<Transform> activatedTransform = new List<Transform>();
    public List<Transform> inActivatedTransform = new List<Transform>();

    // Coroutines
    private Coroutine deActiveTransformCouroutine;

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
        player = GamePlayManager.instance.player;

        if (deActiveTransformCouroutine != null) StopCoroutine(deActiveTransformCouroutine);

        if (createdBooster != null)
        {
            createdBooster.Despawn();
            createdBooster = null;
        }
    }

    public void SpawnBooster(int quantity)
    {
        inActivatedTransform.Clear();
        inActivatedTransform = FindNearbySpawnPoints(player.characterTransform);

        if (inActivatedTransform.Count > quantity)
        {
            int randomNumber = 0;

            for (int i = 0; i < quantity; i++)
            {
                // Pick random position that not in activated position list
                randomNumber = Random.Range(0, inActivatedTransform.Count);

                Transform targetTransform = inActivatedTransform[randomNumber];
                inActivatedTransform.Remove(targetTransform);

                activatedTransform.Add(targetTransform);
                deActiveTransformCouroutine = StartCoroutine(DeActiveTargetTransformAfterTime(targetTransform, 15f));

                Vector3 randomPosition = targetTransform.position;

                // Spawn Booster
                randomNumber = Random.Range(0, prefabs.Count);

                AbstractBooster booster = Instantiate(prefabs[randomNumber], randomPosition, Quaternion.identity, holder);

                booster.Spawn();

                createdBooster = booster;
            }
        }
    }

    private IEnumerator DeActiveTargetTransformAfterTime(Transform target, float time)
    {
        yield return new WaitForSeconds(time);
        activatedTransform.Remove(target);
        inActivatedTransform.Add(target);
        createdBooster.Despawn();
        createdBooster = null;
        yield return new WaitForSeconds(2f);
        SpawnBooster(1);
    }

    public List<Transform> FindNearbySpawnPoints(Transform targetTransform)
    {
        List<Transform> nearbySpawnPoints = new List<Transform>();

        float distanceToDetectTargetObject = 65f;
        Collider[] spawnPointsInRange = Physics.OverlapSphere(targetTransform.position, distanceToDetectTargetObject, spawnPointLayer);

        if (spawnPointsInRange.Length > 0)
        {
            foreach (Collider spawnPoint in spawnPointsInRange)
            {
                nearbySpawnPoints.Add(Cache.GetSpawnpoint(spawnPoint));
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
