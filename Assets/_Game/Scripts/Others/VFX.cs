using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : GameUnit
{
    [SerializeField] private ParticleSystem particle;

    public ParticleSystem GetParticleSystem()
    {
        return particle;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
