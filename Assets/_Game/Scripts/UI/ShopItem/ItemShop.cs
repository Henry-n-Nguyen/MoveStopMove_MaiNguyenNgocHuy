using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;

public class ItemShop : MonoBehaviour
{
    [Header("EquipmentSODatas")]
    [SerializeField] protected EquipmentSODatas equipmentSODatas;

    [Header("References")]
    [SerializeField] protected GameObject itemGameObject;
    [SerializeField] protected Button button;

    public EquipmentType Type { get; protected set; }
    public EquipmentId Id { get; protected set; }
    public int Price { get; protected set; }

    public Image icon;

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {

    }

    public virtual void Spawn()
    {
        itemGameObject.SetActive(true);
    }

    public virtual void Despawn()
    {
        itemGameObject.SetActive(false);
    }
} 
