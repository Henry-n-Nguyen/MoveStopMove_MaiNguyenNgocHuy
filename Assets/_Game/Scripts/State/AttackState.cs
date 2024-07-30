using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.ReadyToAttack();
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
