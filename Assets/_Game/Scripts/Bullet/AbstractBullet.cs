using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public abstract class AbstractBullet : GameUnit
{
    [SerializeField] protected GameObject bulletGameObject;
    [SerializeField] protected Transform bulletTransform;
    [SerializeField] protected Transform meshTransform;

    public AbstractCharacter owner;

    protected float speed = 12f;
    protected float attackRange = 7.5f;

    private bool isSpecialLaunch = false;

    private void OnEnable()
    {
        OnInit();
    }

    private void Update()
    {
        if (isSpecialLaunch)
        {
            SpecialLaunch();
        }
        else
        {
            Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
        CollideWithWall(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == owner) return;

        SimplePool.Spawn<VFX>(PoolType.Particle_HitVFX, character.characterTransform.position, Quaternion.identity);

        character.ChangeState(AbstractCharacter.DEAD_STATE);

        owner.OnPointChange(owner.point + 1);

        owner.GetRadarObject().RemoveTarget(character);

        //OnDespawn();
    }

    private void CollideWithWall(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_WALL)) return;
        OnDespawn();
    }

    protected virtual void OnInit()
    {
        bulletTransform.localPosition = Vector3.zero;

        if (owner != null)
        {
            isSpecialLaunch = owner.isHugeBulletBoosted;
            attackRange = owner.attackRange;
            bulletTransform.localScale = Vector3.one * owner.scaleRatio;
        }
    }

    protected virtual void Throw()
    {

    }

    protected void SpecialLaunch()
    {
        float distance = Time.deltaTime * speed * 2f;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= (distance / 2); // Double bullet exist time // Increase Scale over time
        bulletTransform.localScale += Vector3.one * distance;

        if (attackRange <= 0) OnDespawn();
    }

    public void OnDespawn()
    {
        if (isSpecialLaunch)
        {
            if (attackRange > 0 )
            {
                return;
            }
        }

        SimplePool.Despawn(this);
        bulletTransform.localPosition = Vector3.zero;
    }

    public void SetOwner(AbstractCharacter character)
    {
        this.owner = character;
        bulletTransform.forward = character.characterTransform.forward;
    }
}
