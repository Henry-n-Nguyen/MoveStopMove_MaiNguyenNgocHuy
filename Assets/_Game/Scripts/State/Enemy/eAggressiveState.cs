using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using static UnityEngine.GraphicsBuffer;
using System.Security.Cryptography;

public class eAggressiveState : IState<AbstractCharacter>
{
    float patrolingTimer = 0;

    public void OnEnter(AbstractCharacter t)
    {
        patrolingTimer = 0;
    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Moving();

        Enemy bot = t.GetComponent<Enemy>();

        if (!bot.desPointSet)
        {
            bot.ChangeState(new IdleState());
        }
        else
        {
            patrolingTimer += Time.deltaTime;

            if (patrolingTimer > bot.currentConfigSO.PatrolTime)
            {
                bot.desPointSet = false;
            }
            else
            {
                if (bot.target == null || bot.target.enabled == false)
                {
                    bot.SearchDesPoint();
                    bot.agent.SetDestination(bot.desPoint);
                }
                else
                {
                    bot.agent.SetDestination(bot.desPoint);
                }
            }
        }

        Vector3 distanceToDesPoint = bot.characterTransform.position - bot.desPoint;

        if (distanceToDesPoint.magnitude < 1f)
        {
            bot.ChangeState(new IdleState());

            bot.desPointSet = false;
            patrolingTimer = 0;
        }
    }

    public void OnExit(AbstractCharacter t)
    {

    }

}
