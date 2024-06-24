using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AbstractCharacter
{
    private enum BotType
    {
        AggressiveBot, // Find and positive attack characters around him
        PatrolBot, // Just chilling in his patrol progress
    }

    [Header("Enemy Config")]
    [SerializeField] private EnemyConfigSO[] enemyConfigSOs;
    [HideInInspector] public EnemyConfigSO currentConfigSO = null;

    [Header("Pre-Setup")]
    [SerializeField] private GameObject targetedMark;

    [Header("Patroling")]
    [HideInInspector] public Vector3 desPoint;
    [HideInInspector] public bool desPointSet;
    [HideInInspector] public float desPointRange;

    [Header("BotType")]
    [SerializeField] private BotType typeOfBot;
    [SerializeField] private LayerMask characterLayer;

    [Header("Target")]
    [HideInInspector] public AbstractCharacter target = null;

    // Private variables
    private bool isDetectedTarget;
    private float idlingTimer = 0;

    private Coroutine despawnCoroutine = null;

    private void OnTriggerEnter(Collider other)
    {
        CollideWithPlayerRadar(other);
    }

    private void CollideWithPlayerRadar(Collider other)
    {
        if (other.CompareTag(Constant.TAG_RADAR))
        {
            IsTargeted(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EndCollideWithPlayerRadar(other);
    }

    private void EndCollideWithPlayerRadar(Collider other)
    {
        if (other.CompareTag(Constant.TAG_RADAR))
        {
            IsTargeted(false);
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        if (despawnCoroutine != null) StopCoroutine(despawnCoroutine);

        desPointSet = false;

        isDetectedTarget = false;

        RandomTypeOfBot();

        agent.speed = moveSpeed * currentConfigSO.AgentSpeedConvertRate;

        IsTargeted(false);
    }

    private void RandomTypeOfBot() {
        // Random type of this bot
        typeOfBot = (BotType)Random.Range(0, 2);

        currentConfigSO = enemyConfigSOs[(int) typeOfBot];
    }

    // State Methods
    public override void Moving()
    {
        base.Moving();

        if (targetsInRange.Count > 0)
        {
            if (isReadyToAttack && !isDetectedTarget)
            {
                agent.SetDestination(characterTransform.position);

                desPointSet = false;

                ChangeState(new AttackState());
            }
        }
    }

    public void SearchDesPoint()
    {
        switch (typeOfBot)
        {
            case BotType.AggressiveBot:
                Aggressive();
                break;

            case BotType.PatrolBot:
                Patrol();
                break;
        }

        desPointSet = true;
    }

    private void Aggressive()
    {
        float minDistance = Mathf.Infinity;

        List<AbstractCharacter> characterList = new List<AbstractCharacter>();

        characterList.AddRange(BotPool.GetActivatedBotList());
        characterList.Add(GamePlayManager.instance.player);

        if (characterList.Count > 0)
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                AbstractCharacter foundedTarget = characterList[i];

                if (foundedTarget.index != index)
                {
                    float currentDistanceSq = Vector3.SqrMagnitude(characterTransform.position - foundedTarget.characterTransform.position);

                    if (currentDistanceSq < minDistance)
                    {
                        desPoint = foundedTarget.characterTransform.position;
                        target = foundedTarget;
                    }
                }
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // create random point in a walkRadius
        Vector3 randomDirection = Random.insideUnitSphere * desPointRange;

        // reference point was created to current tranform and find random point with it
        randomDirection += characterTransform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, desPointRange, 1);

        // return result
        desPoint = hit.position;
    }

    public override void StopMoving()
    {
        base .StopMoving();

        agent.SetDestination(characterTransform.position);

        if (isOnPause)
        {
            desPointSet = false;
            return;
        }

        if (targetsInRange.Count > 0)
        {
            if (isReadyToAttack && !isDetectedTarget)
            {
                idlingTimer = 0;
                desPointSet = false;

                ChangeState(new AttackState());
            }
        }

        if (!desPointSet)
        {
            idlingTimer += Time.deltaTime;

            if (idlingTimer > currentConfigSO.IdleTime)
            {
                SearchDesPoint();

                isDetectedTarget = false;
                idlingTimer = 0;

                switch (typeOfBot)
                {
                    case BotType.PatrolBot: ChangeState(new ePatrolState()); break;
                    case BotType.AggressiveBot: ChangeState(new eAggressiveState()); break;
                }
            }
        }
    }

    public override void ReadyToAttack()
    {
        if (!isDetectedTarget)
        {
            TurnTowardClosestCharacter();

            base.ReadyToAttack();

            isDetectedTarget = true;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Dead()
    {
        if (!isDead)
        {
            agent.SetDestination(characterTransform.position);

            base.Dead();

            isDead = true;
            GamePlayManager.instance.CharacterDied();

            despawnCoroutine = StartCoroutine(DespawnEnemyAfterTime(2.5f));
        }
    }

    private IEnumerator DespawnEnemyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        BotPool.Despawn(this);

        BotGenerator.instance.SpawnBots();
    }

    public void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }
}
