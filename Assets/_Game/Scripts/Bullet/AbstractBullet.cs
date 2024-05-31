using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    [SerializeField] protected GameObject bulletGameObject;
    [SerializeField] protected Transform bulletTransform;
    [SerializeField] protected Transform meshTransform;

    public event Action OnBulletSpawn;

    public int id;

    public AbstractCharacter owner;

    protected float speed = 12f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

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
            Launch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithCharacter(other);
        CollideWithWall(other);
    }

    private void CollideWithCharacter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            AbstractCharacter character = Cache.GetCharacter(other);
            if (character != owner)
            {
                character.ChangeState(new DeadState());

                owner.point++;
                owner.OnPointChange();
                owner.targetsInRange.Remove(character);

                Despawn();
            }
        }
    }

    private void CollideWithWall(Collider other)
    {
        if (other.CompareTag(Constant.TAG_VEHICLE))
        {
            Despawn();
        }
    }

    protected virtual void OnInit()
    {
        isSpecialLaunch = owner.isBoosted;

        attackRange = owner.GetAttackRange();
        scaleRatio = owner.GetScaleParametters();

        bulletTransform.localScale = Vector3.one * scaleRatio;

        OnBulletSpawn += owner.IsReadyToAttack;
    }

    protected virtual void Launch()
    {

    }

    protected void SpecialLaunch()
    {
        float distance = Time.deltaTime * speed * 2f;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= (distance / 2); // Double bullet exist time
        scaleRatio += distance; // Increase Scale over time
        bulletTransform.localScale = Vector3.one * scaleRatio;

        if (attackRange <= 0) Despawn();
    }

    public void Spawn()
    {
        bulletTransform.position = owner.GetAttackPoint().position;
        bulletTransform.rotation = owner.characterTransform.rotation;
        bulletGameObject.SetActive(true);

        OnBulletSpawn?.Invoke();
    }

    public void Despawn()
    {
        if (isSpecialLaunch)
        {
            if (attackRange > 0 )
            {
                return;
            }
        }

        bulletGameObject.SetActive(false);
        bulletTransform.localPosition = Vector3.zero;

        OnBulletSpawn?.Invoke();
    }

    public void SpecialBuff()
    {
        scaleRatio += Time.deltaTime;
    }
}
