using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AbstractCharacter
{
    private enum BotType
    {
        AgressiveBot, // Find and positive attack characters around him
        PatrolBot, // Just chilling in his patrol progress
    }


    [Header("Pre-Setup")]
    [SerializeField] private GameObject targetedMark;

    [Header("Patroling")]
    [SerializeField] private Vector3 desPoint;
    [SerializeField] private bool desPointSet;
    [SerializeField] private float desPointRange;

    [Header("BotType")]
    [SerializeField] private BotType typeOfBot;
    [SerializeField] private LayerMask characterLayer;

    [Header("Target")]
    [SerializeField] private AbstractCharacter target = null;

    // Private variables
    private bool isDetectedTarget;
    private float patrolingTimer = 0;
    private float idlingTimer = 0;


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

        StopAllCoroutines();

        desPointSet = false;

        isDetectedTarget = false;

        agent.speed = moveSpeed * 0.67f;

        RandomTypeOfBot();

        IsTargeted(false);
    }

    private void RandomTypeOfBot() {
        // Random type of this bot
        typeOfBot = (BotType)Random.Range(0, 2);
    }

    public override void Moving()
    {
        base.Moving();

        if (targetsInRange.Count > 0)
        {
            if (isReadyToAttack && !isDetectedTarget)
            {
                agent.SetDestination(characterTransform.position);

                desPointSet = false;
                patrolingTimer = 0;

                ChangeState(new AttackState());
            }
        }

        if (!desPointSet)
        {
            ChangeState(new IdleState());
        }
        else
        {
            patrolingTimer += Time.deltaTime;

            if (patrolingTimer > 5f)
            {
                desPointSet = false;
                patrolingTimer = 0;
            }

            if (typeOfBot == BotType.AgressiveBot)
            {
                if (target == null || target.enabled == false)
                {
                    SearchDesPoint();
                    agent.SetDestination(desPoint);
                }
                else
                {
                    agent.SetDestination(desPoint);
                }
            }
            else
            {
                agent.SetDestination(desPoint);
            }
        }

        Vector3 distanceToDesPoint = characterTransform.position - desPoint;

        if (distanceToDesPoint.magnitude < 1f) 
        {
            ChangeState(new IdleState());

            desPointSet = false; 
            patrolingTimer = 0;

        }
    }

    private void SearchDesPoint()
    {
        switch (typeOfBot)
        {
            case BotType.AgressiveBot:
                Agressive();
                break;

            case BotType.PatrolBot:
                Patrol();
                break;
        }

        desPointSet = true;
    }

    private void Agressive()
    {
        float minDistance = Mathf.Infinity;

        Collider[] characterCollides = Physics.OverlapSphere(characterTransform.position, 40f, characterLayer);

        if (characterCollides.Length > 0)
        {
            for (int i = 0; i < characterCollides.Length; i++)
            {
                AbstractCharacter foundedTarget = Cache.GetCharacter(characterCollides[i]);

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


    // State Methods
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

            if (idlingTimer > 0.5f)
            {
                SearchDesPoint();

                isDetectedTarget = false;
                idlingTimer = 0;

                ChangeState(new PatrolState());
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

            StartCoroutine(DespawnEnemy(2.5f));

            isDead = true;
        }
    }

    private IEnumerator DespawnEnemy(float time)
    {
        yield return new WaitForSeconds(time);

        BotPool.Despawn(this);
        GamePlayManager.instance.CharacterDied();

        BotGenerator.instance.SpawnBots();
    }

    public void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }
}
