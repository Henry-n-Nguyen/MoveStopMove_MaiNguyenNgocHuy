using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.WSA;

public abstract class AbstractBullet : MonoBehaviour
{
    [SerializeField] protected GameObject bulletGameObject;
    [SerializeField] protected Transform bulletTransform;
    [SerializeField] protected Transform meshTransform;

    public event Action OnBulletSpawn;

    public int id;

    public AbstractCharacter owner;

    protected float speed = 6f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

    private void OnEnable()
    {
        OnInit();
    }

    private void Update()
    {
        Launch();
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
        attackRange = owner.GetAttackRange();
        scaleRatio = owner.GetScaleParametters();

        bulletTransform.localScale = Vector3.one * scaleRatio;

        OnBulletSpawn += owner.IsReadyToAttack;
    }

    protected virtual void Launch()
    {

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
        bulletGameObject.SetActive(false);
        bulletTransform.localPosition = Vector3.zero;

        OnBulletSpawn?.Invoke();
    }

    
}
