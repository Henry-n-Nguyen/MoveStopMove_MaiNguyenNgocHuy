using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;

    public List<Material> pantMaterialList = new List<Material>();
    public List<Material> skinMaterialList = new List<Material>();

    private void Awake()
    {
        instance = this;
    }
}
