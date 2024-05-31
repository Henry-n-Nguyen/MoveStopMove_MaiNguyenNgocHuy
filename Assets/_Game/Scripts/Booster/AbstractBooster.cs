using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBooster : MonoBehaviour
{
    const string TRIGGER_SPAWN = "spawn";

    [Header("Set-up")]
    [SerializeField] private GameObject boosterGameObject;

    [SerializeField] private Animator anim;

    private void OnEnable()
    {
        anim.ResetTrigger(TRIGGER_SPAWN);
        anim.SetTrigger(TRIGGER_SPAWN);

        StartCoroutine(SelfDestroy(15f));
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            AbstractCharacter character = Cache.GetCharacter(other);

            if (!character.isBoosted)
            {
                TriggerBoost(character);
                character.CheckBoost();
            }

            Despawn();
        }
    }

    protected virtual void TriggerBoost(AbstractCharacter character)
    {

    }

    public void Spawn()
    {
        boosterGameObject.SetActive(true);
    }

    public void Despawn()
    {
        Destroy(boosterGameObject);
    }

    private IEnumerator SelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Despawn();
    }
}
