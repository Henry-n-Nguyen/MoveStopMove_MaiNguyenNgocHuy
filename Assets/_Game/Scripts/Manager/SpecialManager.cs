using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialManager : MonoBehaviour
{
    public static SpecialManager instance;

    [SerializeField] private List<Special> specialList = new List<Special>();
    [SerializeField] private List<Sprite> specialSpriteList = new List<Sprite>();

    private void Awake()
    {
        instance = this;
    }

    public Special GetSpecialById(int id)
    {
        id++;

        foreach (Special special in specialList)
        {
            if (special.id == id) return special;
        }

        return specialList[id] != null ? specialList[id] : specialList[0];
    }

    public List<Special> GetWeaponList()
    {
        return specialList;
    }

    public List<Sprite> GetSpecialSpriteList()
    {
        return specialSpriteList;
    }
}
