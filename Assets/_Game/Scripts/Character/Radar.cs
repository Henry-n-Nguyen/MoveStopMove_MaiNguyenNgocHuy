using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Radar : MonoBehaviour
{
    [SerializeField] private AbstractCharacter owner;

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == owner) return;
        if (!character.IsDead) owner.targetsInRange.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        EndCollideWithCharacter(other);
    }

    private void EndCollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;
        AbstractCharacter character = Cache.GetCharacter(other);

        if (character.gameObject == owner) return;
        int indexOfTarget = owner.targetsInRange.IndexOf(character);
        if (indexOfTarget != -1) owner.targetsInRange.RemoveAt(indexOfTarget); 
    }
}
