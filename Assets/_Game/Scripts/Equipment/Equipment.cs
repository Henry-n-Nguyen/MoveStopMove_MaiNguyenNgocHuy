using HuySpace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipmentId id;
    public int price;
    public Sprite sprite;
    public Material materialColor;

    public bool IsActive => gameObject.activeSelf;  

    public void Init(EquipmentId id, int price, Sprite sprite, Material color)
    {
        this.id = id;
        this.price = price;
        this.sprite = sprite;
        this.materialColor = color;
    }

    public virtual void Spawn()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    public virtual void Despawn()
    {
        gameObject.SetActive(false);
    }
}
