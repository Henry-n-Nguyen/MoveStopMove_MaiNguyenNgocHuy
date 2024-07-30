using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/EnemyConfigSO")]
public class EnemyConfigSO : ScriptableObject
{
    public BotType type;
    [field: SerializeField] public float AgentSpeedConvertRate { get; private set; } = 0.67f;
    [field: SerializeField] public float IdleTime { get; private set; } = 0.5f;
    [field: SerializeField] public float PatrolTime { get; private set; } = 5f;
}
