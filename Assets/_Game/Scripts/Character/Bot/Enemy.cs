using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AbstractCharacter
{
    [Header("Pre-Setup")]
    [SerializeField] private GameObject targetedMark;

    [Header("Patroling")]
    [SerializeField] private Vector3 desPoint;
    [SerializeField] private bool desPointSet;
    [SerializeField] private float desPointRange;

    [SerializeField] private bool isDetectedTarget;

    private float patrolingTimer = 0;
    private float idlingTimer = 0;

    public override void OnInit()
    {
        base.OnInit();

        isDetectedTarget = false;

        agent.speed = moveSpeed * 0.67f;
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

            agent.SetDestination(desPoint);
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
        // create random point in a walkRadius
        Vector3 randomDirection = Random.insideUnitSphere * desPointRange;

        // reference point was created to current tranform and find random point with it
        randomDirection += characterTransform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, desPointRange, 1);
        
        // return result
        desPoint = hit.position;

        desPointSet = true;
    }

    public override void StopMoving()
    {
        base .StopMoving();

        agent.SetDestination(characterTransform.position);

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

    public override void Attack()
    {
        if (!isDetectedTarget)
        {
            TurnTowardClosestCharacter();

            base.Attack();

            isDetectedTarget = true;
        }
    }

    public override void Dead()
    {
        if (!isDead)
        {
            base.Dead();

            StartCoroutine(DespawnEnemy(3f));

            isDead = true;
        }
    }

    private IEnumerator DespawnEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        BotGenerator.instance.SpawnBot(1);
        BotPool.Despawn(this);
    }

    public void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }
}
