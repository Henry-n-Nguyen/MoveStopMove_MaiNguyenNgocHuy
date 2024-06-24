using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

[CreateAssetMenu(menuName = "SciptableObjects/Others/ParticleDataSO")]
public class ParticleDataSO : ScriptableObject
{
    [SerializeField] private ParticleSystem[] particles;
    
    public ParticleSystem GetParticle(ParticleType type)
    {
        return particles[(int)type];
    }
}
