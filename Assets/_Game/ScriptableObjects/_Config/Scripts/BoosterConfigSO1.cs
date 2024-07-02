using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/BoosterConfigSO")]
public class BoosterConfigSO : ScriptableObject
{
    [Header("Unique")]
    [SerializeField] private float stat = 0;

    public float Stat => stat;
}
