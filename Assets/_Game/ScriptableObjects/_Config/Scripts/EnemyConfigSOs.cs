using HuySpace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/EnemyConfigSOs")]
public class EnemyConfigSOs : ScriptableObject
{
    [SerializeField] private List<EnemyConfigSO> enemyConfigSOs = new List<EnemyConfigSO>();

    public EnemyConfigSO GetData(BotType type)
    {
        foreach (EnemyConfigSO config in enemyConfigSOs)
        {
            if (config.type == type) return config;
        }

        return null;
    }
}
