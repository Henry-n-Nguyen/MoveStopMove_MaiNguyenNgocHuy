using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class PoolController : MonoBehaviour
{
    [Header("Pool")]
    public List<PoolAmount> PoolWithRoot;

    void Awake()
    {
        int quantity = LevelManager.Ins.startCharacterAmount;

        for (int i = 0; i < PoolWithRoot.Count; i++)
        {
            SimplePool.Preload(PoolWithRoot[i].prefab, PoolWithRoot[i].amount, PoolWithRoot[i].root);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;

    public PoolAmount(Transform root, GameUnit prefab, int amount)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
    }
}
