using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuySpace;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public ParticleSystem hitVFX;
    public ParticleSystem boostedVFX;

    private void Awake()
    {
        instance = this;
    }
}
