using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public int id;

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
