using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponGameObject;
    public int id;

    public void Spawn()
    {
        weaponGameObject.SetActive(true);
    }

    public void Despawn()
    {
        weaponGameObject.SetActive(false);
    }
}
