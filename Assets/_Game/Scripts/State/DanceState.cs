using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Dance();
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
