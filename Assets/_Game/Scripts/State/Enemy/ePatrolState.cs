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

    }

    public void OnExecute(AbstractCharacter t)
    {
        t.Moving();

        Enemy bot = (Enemy) t;

        if (bot.GetRadarObject().IsAnyTargetInRange)
        {
            bot.desPointSet = false;
            bot.patrolingTimer = 0;

            bot.ChangeState(AbstractCharacter.ATTACK_STATE);
        }

        if (!bot.desPointSet)
        {
            bot.ChangeState(AbstractCharacter.IDLE_STATE);
        }
        else
        {
            bot.patrolingTimer += Time.deltaTime;

            if (bot.patrolingTimer > bot.currentConfigSO.PatrolTime)
            {
                bot.desPointSet = false;
                bot.patrolingTimer = 0;
            }

            bot.GetAgent().SetDestination(bot.desPoint);
        }


        Vector3 distanceToDesPoint = bot.characterTransform.position - bot.desPoint;

        if (distanceToDesPoint.magnitude < 1f)
        {
            bot.desPointSet = false;
            bot.patrolingTimer = 0;

            bot.ChangeState(AbstractCharacter.IDLE_STATE);
        }
    }

    public void OnExit(AbstractCharacter t)
    {

    }
}
