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

    private bool isDetectedTarget;

    public override void OnInit()
    {
        base.OnInit();

        isDetectedTarget = false;
    }

    public override void Moving()
    {
        base.Moving();

        if (!desPointSet)
        {
            ChangeState(new IdleState());
        }
        else
        {
            agent.SetDestination(desPoint);
        }

        if (targetsInRange.Count > 0)
        {
            ChangeState(new IdleState());
        }

        Vector3 distanceToDesPoint = characterTransform.position - desPoint;

        if (distanceToDesPoint.magnitude < 1f) 
        {   
            desPointSet = false; 
        }
    }

    private void SearchDesPoint()
    {
        // Calculate random number x, z axis
        float randomZ = Random.Range(-desPointRange, desPointRange);
        float randomX = Random.Range(-desPointRange, desPointRange);

        desPoint = characterTransform.position + Vector3.right * randomX + Vector3.forward * randomZ;

        desPointSet = true;
    }

    public override void StopMoving()
    {
        base .StopMoving();

        agent.SetDestination(characterTransform.position);

        if (targetsInRange.Count > 0)
        {
            TurnTowardClosestCharacter();

            if (isReadyToAttack && !isDetectedTarget)
            {
                ChangeState(new AttackState());
            }
        }

        if (!desPointSet)
        {
            SearchDesPoint();

            ChangeState(new PatrolState());
        }
    }

    public override void Attack()
    {
        if (!isDetectedTarget)
        {
            base.Attack();

            isDetectedTarget = true;
        }
    }

    public void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }
}
