using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private AbstractCharacter owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER) && other.gameObject != owner.gameObject)
        {
            AbstractCharacter character = other.gameObject.GetComponent<AbstractCharacter>();
            if (!character.IsDead) owner.targetsInRange.Add(character);
        }

        if (other.gameObject.CompareTag(Constant.TAG_ENEMY))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!enemy.IsDead) enemy.IsTargeted(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER) && other.gameObject != owner.gameObject)
        {
            int indexOfTarget = owner.targetsInRange.IndexOf(other.gameObject.GetComponent<AbstractCharacter>());
            if (indexOfTarget != -1) owner.targetsInRange.RemoveAt(indexOfTarget);
        }

        if (other.gameObject.CompareTag(Constant.TAG_ENEMY))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.IsTargeted(false);
        }
    }
}
