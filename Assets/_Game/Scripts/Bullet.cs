using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bulletTransform;

    public int id;

    public Character owner;

    [SerializeField] private float speed = 2f;

    private float attackRange = 7.5f;
    private float scaleRatio = 1f;

    private void OnEnable()
    {
        OnInit();
    }

    private void Update()
    {
        Fly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER) && other.gameObject != owner.gameObject)
        {
            Despawn();
        }
    }

    public void OnInit()
    {
        attackRange = owner.GetAttackRange();
        scaleRatio = owner.GetScaleParametters();
    }

    private void Fly()
    {
        float distance = speed * Time.deltaTime;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= distance;

        if (attackRange <= 0) Despawn();
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
