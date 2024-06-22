using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/EnemyConfigSO")]
public class EnemyConfigSO : ScriptableObject
{
    [Header("Unique")]
    [SerializeField] private float agentSpeedConvertRate = 0.67f;
    [SerializeField] private float idleTime = 0.5f;
    [SerializeField] private float patrolTime = 5f;

    public float AgentSpeedConvertRate => agentSpeedConvertRate;
    public float IdleTime => idleTime;
    public float PatrolTime => patrolTime;
}
