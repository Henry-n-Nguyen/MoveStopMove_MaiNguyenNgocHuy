using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [field :SerializeField] public int capacity { get; private set; }

    [field :SerializeField] public int startCharacterAmount { get; private set; }
    [field :SerializeField] public float UsableLength { get; private set; } = 0f;
    [field :SerializeField] public float UsebleWidth { get; private set; } = 0f;
    [field :SerializeField] public float UsableHeight { get; private set; } = 0f;
}
