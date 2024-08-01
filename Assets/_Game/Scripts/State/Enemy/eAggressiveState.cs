using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using System.Security.Cryptography;
using UnityEngine.UIElements;

public class EAggressiveState : IState<AbstractCharacter>
{
    public void OnEnter(AbstractCharacter t)
    {
        Enemy bot = (Enemy)t;

        bot.AggressiveChase();
    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Moving();
    }

    public void OnExit(AbstractCharacter t)
    {

    }

}
