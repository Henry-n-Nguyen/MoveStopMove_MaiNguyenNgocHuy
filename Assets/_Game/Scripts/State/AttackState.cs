using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Character>
{
    float timer = 0f;

    public void OnEnter(Character t)
    {

    }

    public void OnExecute(Character t)
    {
        t.Attack();

        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(Character t)
    {

    }
}
