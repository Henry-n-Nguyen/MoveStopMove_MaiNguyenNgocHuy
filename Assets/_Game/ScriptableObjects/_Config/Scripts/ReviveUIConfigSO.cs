using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/ReviveUIConfigSO")]
public class ReviveUIConfigSO : ScriptableObject
{
    [Header("Unique")]
    [SerializeField] private float waitingTime = 5.5f;
    [SerializeField] private float delayTime = 1f;

    public float WaitingTime => waitingTime;
    public float DelayTime => delayTime;
}
