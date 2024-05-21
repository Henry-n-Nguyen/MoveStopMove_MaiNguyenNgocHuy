using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Dead();
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
