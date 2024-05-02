using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class PatrolState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Moving();
    }

    public void OnExit(AbstractCharacter t)
    {

    }

}
