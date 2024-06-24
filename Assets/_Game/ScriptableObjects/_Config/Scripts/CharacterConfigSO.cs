using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Config/CharacterConfigSO")]
public class CharacterConfigSO : ScriptableObject
{
    [Header("Scale levels")]
    [SerializeField] private int[] levelMilestones = { 3, 7, 15, 24 };
    [SerializeField] private float[] level2Scale = { 1.12f, 1.24f, 1.36f, 1.48f };

    [Header("Raw stats")]
    [SerializeField] private float rawMoveSpeed = 5f;
    [SerializeField] private float rawAttackRange = 7.5f;
    [SerializeField] private float rawScale = 1f;

    public float RawMoveSpeed => rawMoveSpeed;
    public float RawAttackRange => rawAttackRange;
    public float RawScale => rawScale;

    public int GetLevelMilestone(int level)
    {
        return levelMilestones[level];
    }

    public float GetLevel2Scale(int level)
    {
        return level2Scale[level];
    }

    public int GetMaxScaleLevel()
    {
        return levelMilestones.Length;
    }
}
