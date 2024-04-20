using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour
{
    protected const string TRIGGER_RUN = "run";
    protected const string TRIGGER_IDLE = "idle";
    protected const string TRIGGER_ATTACK = "attack";

    // Editor
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected Character chacractedScript;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected SphereCollider radarObject;
    [SerializeField] private Animator anim;

    public int index;

    protected float moveSpeed = 5f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

    protected bool isMoving;
    protected bool isDead;
    
    [SerializeField] private bool isAttack = true;

    public bool IsDead => isDead;

    // Private
    private IState<Character> currentState;

    private string currentAnimName;


    private void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public virtual void OnInit()
    {
        radarObject.radius = attackRange;

        scaleRatio = 1f;

        ChangeState(new IdleState());
    }

    public void ChangeState(IState<Character> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            anim.ResetTrigger(animName);
            anim.SetTrigger(currentAnimName);
        }
    }

    public virtual void Moving()
    {
        ChangeAnim(TRIGGER_RUN);
    }

    public virtual void StopMoving()
    {
        ChangeAnim(TRIGGER_IDLE);
    }

    public virtual void Attack()
    {
        //ChangeAnim(TRIGGER_ATTACK);

        BulletManager.instance.Spawn(chacractedScript);
    }

    public void WarpTo(Vector3 pos)
    {
        agent.Warp(pos);
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetScaleParametters()
    {
        return scaleRatio;
    }

    public int GetWeaponId()
    {
        return weapon.id;
    }

    public Transform GetAttackPoint()
    {
        return attackPoint;
    }
}
