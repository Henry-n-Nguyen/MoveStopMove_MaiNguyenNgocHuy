using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBullet : GameUnit
{
    [SerializeField] private Weapon prefab;

    private float timer = 5f;

    private void OnEnable()
    {
        timer = 5f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerWithPlayer(other);
    }

    private void TriggerWithPlayer(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == null) return;
        if (character.IsDead) return;
        if (character.IsReadyToAttack()) return;

        character.PickUpWeapon(prefab);

        SimplePool.Despawn(this);
    }
}
