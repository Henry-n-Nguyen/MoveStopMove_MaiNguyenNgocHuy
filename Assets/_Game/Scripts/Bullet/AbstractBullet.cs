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

    private void Start()
    {
        SubcribeEvent();
    }

    private void OnEnable()
    {
        OnInit();
    }

    private void SubcribeEvent()
    {
        OnBulletSpawn += owner.IsReadyToAttack;
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
        if (!other.CompareTag(Constant.TAG_CHARACTER)) return;

        AbstractCharacter character = Cache.GetCharacter(other);

        if (character == owner) return;

        character.ChangeState(new DeadState());

        owner.point++;
        owner.OnPointChanges();
        owner.targetsInRange.Remove(character);

        Despawn();
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
        isSpecialLaunch = owner.isHugeBulletBoosted;

        attackRange = owner.attackRange;
        scaleRatio = owner.scaleRatio;

        bulletTransform.localScale = Vector3.one * scaleRatio;

        
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

        OnBulletSpawned();
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

        OnBulletSpawned();
    }

    private void OnBulletSpawned()
    {
        OnBulletSpawn?.Invoke();
    }
}
