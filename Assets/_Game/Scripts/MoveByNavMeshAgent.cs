using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByNavMeshAgent : Character
{
    [SerializeField] private GameObject targetedMark;

    public override void Moving()
    {
        base.Moving();
    }

    public override void StopMoving()
    {
        base .StopMoving();
    }

    public void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }
}