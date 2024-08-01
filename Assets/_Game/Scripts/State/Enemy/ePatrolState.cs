using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System.Security.Cryptography;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EPatrolState : IState<AbstractCharacter>
{

    public void OnEnter(AbstractCharacter t)
    {
        Enemy bot = (Enemy)t;

        bot.PatrolAround();
    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Moving();
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
