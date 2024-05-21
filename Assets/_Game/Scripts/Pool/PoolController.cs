using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using UnityEngine.UIElements;

public class PoolController : MonoBehaviour
{
    [Header("In-Code")]
    [SerializeField] private int quantity;

    [Header("Pre-Setup")]
    [SerializeField] private Transform holder;

    [SerializeField] private Enemy prefab;

    void Awake()
    {
        BotPool.Preload(prefab, quantity, holder);
    }
}
