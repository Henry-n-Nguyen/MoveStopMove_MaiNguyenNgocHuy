using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBooster : GameUnit
{
    const string TRIGGER_SPAWN = "spawn";

    [Header("Set-up")]
    [SerializeField] private GameObject boosterGameObject;

    [SerializeField] private Animator anim;

    private void OnEnable()
    {
        anim.ResetTrigger(TRIGGER_SPAWN);
        anim.SetTrigger(TRIGGER_SPAWN);

        StartCoroutine(SelfDestroyAfterTime(15f));
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (!character.isBoosted) TriggerBoost(character);

        SimplePool.Despawn(this);
    }

    protected virtual void TriggerBoost(AbstractCharacter character)
    {

    }

    private IEnumerator SelfDestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SimplePool.Despawn(this);
    }
}
