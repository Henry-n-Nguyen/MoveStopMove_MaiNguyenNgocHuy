using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<AbstractCharacter>
{
    float timer = 0f;

    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.ReadyToAttack();

        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
