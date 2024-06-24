using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class PoolController : MonoBehaviour
{
    [Header("Pre-Setup")]
    [SerializeField] private Transform holder;

    [SerializeField] private Enemy prefab;

    [Header("Particle")]
    public ParticleAmount[] Particle;

    void Start()
    {
        int quantity = GamePlayManager.instance.characterAmount;
        BotPool.Preload(prefab, quantity, holder);

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].type, Particle[i].amount, Particle[i].root);
        }
    }
}
