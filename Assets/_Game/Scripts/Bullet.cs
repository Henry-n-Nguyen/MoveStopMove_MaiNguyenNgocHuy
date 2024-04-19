using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bulletTransform;

    public Character owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public int id;

    private float speed = 2f;
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

    public void OnInit()
    {
        //attackRange = owner.GetAttackRange();
        attackRange = 7.5f;
        //scaleRatio = owner.GetScaleParametters();
        scaleRatio = 1f;
    }

    private void Fly()
    {
        bulletTransform.position += bulletTransform.forward * speed * Time.deltaTime;
        attackRange -= speed * Time.deltaTime;
        if (attackRange <= 0) Despawn();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
