using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Radar : MonoBehaviour
{
    [SerializeField] private AbstractCharacter owner;
    [SerializeField] protected SphereCollider radarCollider;

    private List<AbstractCharacter> targetsInRange = new List<AbstractCharacter>();

    public bool IsAnyTargetInRange => targetsInRange.Count > 0;

    [field: SerializeField] public AbstractCharacter nearestTarget { get; private set; } = null;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        radarCollider.radius = owner.attackRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == owner) return;

        if (!character.IsDead) targetsInRange.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        EndCollideWithCharacter(other);
    }

    private void EndCollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == owner) return;

        targetsInRange.Remove(character);

        if (character == nearestTarget)
        {
            nearestTarget = null;
        }
    }

    // Public function
    public AbstractCharacter FindClosestCharacter()
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 ownerPos = owner.characterTransform.position;

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < targetsInRange.Count; i++)
        {
            AbstractCharacter target = targetsInRange[i];
            targetPos = target.characterTransform.position;

            if (target.IsDead || Vector3.Distance(targetPos, ownerPos) >= owner.attackRange)
            {
                targetsInRange.RemoveAt(i);
                continue;
            }

            float distanceSq = Vector3.SqrMagnitude(targetPos - ownerPos);

            if (distanceSq < minDistance)
            {
                minDistance = distanceSq;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }

    public void ClearTargetList()
    {
        targetsInRange.Clear();
    }

    public void AddTarget(AbstractCharacter target)
    {
        targetsInRange.Add(target);
    }

    public void RemoveTarget(AbstractCharacter target)
    {
        targetsInRange.Remove(target); 
    }

    public bool IsTargetInRange(AbstractCharacter target)
    {
        if (!IsAnyTargetInRange) return false;

        return targetsInRange.Contains(target);
    }

    public void OnOwnerRevive()
    {
        foreach (AbstractCharacter character in targetsInRange)
        {
            character.GetRadarObject().AddTarget(owner);
        }
    }
}
