using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour
{
    protected const string TRIGGER_RUN = "run";
    protected const string TRIGGER_IDLE = "idle";

    // Editor
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private Animator anim;

    protected float moveSpeed = 5f;
    protected bool isMoving;

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

    public void WarpTo(Vector3 pos)
    {
        agent.Warp(pos);
    }
}
