using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;

public abstract class AbstractBullet : MonoBehaviour
{
    [SerializeField] protected Transform bulletTransform;
    [SerializeField] protected Transform meshTransform;

    public int id;

    public AbstractCharacter owner;

    [SerializeField] protected float speed = 2f;

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
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER) && other.gameObject != owner.gameObject)
        {
            Despawn();
        }
    }

    protected virtual void OnInit()
    {
        attackRange = owner.GetAttackRange();
        scaleRatio = owner.GetScaleParametters();
    }

    protected virtual void Launch()
    {
        
    }

    public void Spawn()
    {
        bulletTransform.position = owner.GetAttackPoint().position;
        bulletTransform.rotation = owner.transform.rotation;
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
