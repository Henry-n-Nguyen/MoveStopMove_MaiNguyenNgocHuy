using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using UnityEngine.UIElements;

public class PoolController : MonoBehaviour
{
    [Header("Pre-Setup")]
    [SerializeField] private Transform holder;

    [SerializeField] private Enemy prefab;

    void Start()
    {
        int quantity = GamePlayManager.instance.characterAmount;
        BotPool.Preload(prefab, quantity, holder);
    }
}
