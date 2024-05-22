using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    public int id;

    public Hat specialHat = null;
    public Skin specialSkin = null;
    public Pant specialPant = null;

    public GameObject wing = null;
    public GameObject tail =  null;
    public GameObject leftHandAccessory = null;

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
