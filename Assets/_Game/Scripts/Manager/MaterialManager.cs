using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;

    [SerializeField] private List<Pant> pantList = new List<Pant>();
    [SerializeField] private List<Sprite> pantSpriteList = new List<Sprite>();
    [SerializeField] private List<Skin> skinList = new List<Skin>();
    [SerializeField] private List<Sprite> skinSpriteList = new List<Sprite>();

    private void Awake()
    {
        instance = this;
    }

    public Pant GetPantById(int id)
    {
        foreach (Pant pant in pantList)
        {
            if (pant.id == id) return pant;
        }

        return pantList[id] != null ? pantList[id] : pantList[0];
    }

    public List<Pant> GetPantList()
    {
        return pantList;
    }
    public List<Sprite> GetPantSpriteList()
    {
        return pantSpriteList;
    }

    public Skin GetSkinById(int id)
    {
        foreach (Skin skin in skinList)
        {
            if (skin.id == id) return skin;
        }

        return skinList[id] != null ? skinList[id] : skinList[0];
    }

    public List<Skin> GetSkinList()
    {
        return skinList;
    }

    public List<Sprite> GetSkinSpriteList()
    {
        return skinSpriteList;
    }

}
