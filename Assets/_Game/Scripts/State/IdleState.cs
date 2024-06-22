using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class IdleState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.StopMoving();
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
