using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour
{
    // Editor
    [SerializeField] protected Transform characterTransform;
    [SerializeField] protected Character characterScript;
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
    protected bool isReadyToAttack;

    public bool IsDead => isDead;

    // List target
    [SerializeField] protected List<Character> targetsInRange = new List<Character>();

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
        isReadyToAttack = IsReadyToAttack();

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER))
        {
            Character character = other.gameObject.GetComponent<Character>();
            targetsInRange.Add(character);
        }

        if (other.gameObject.CompareTag(Constant.TAG_ENEMY))
        {
            MoveByNavMeshAgent enemy = other.GetComponent<MoveByNavMeshAgent>();
            enemy.IsTargeted(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_CHARACTER))
        {
            targetsInRange.RemoveAt(targetsInRange.IndexOf(other.gameObject.GetComponent<Character>()));
        }

        if (other.gameObject.CompareTag(Constant.TAG_ENEMY))
        {
            MoveByNavMeshAgent enemy = other.GetComponent<MoveByNavMeshAgent>();
            enemy.IsTargeted(false);
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

    public bool IsReadyToAttack()
    {
        if (BulletManager.instance.IsBulletActivated(index)) { 
            weapon.Despawn();
            return false;
        }
        else
        {
            weapon.Spawn();
            return true;
        }
    }

    public virtual void Moving()
    {
        ChangeAnim(Constant.TRIGGER_RUN);
    }

    public virtual void StopMoving()
    {
        ChangeAnim(Constant.TRIGGER_IDLE);
    }

    public virtual void Attack()
    {
        ChangeAnim(Constant.TRIGGER_ATTACK);

        if (isReadyToAttack) BulletManager.instance.Spawn(characterScript);
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

    public void WarpTo(Vector3 pos)
    {
        agent.Warp(pos);
    }
}
