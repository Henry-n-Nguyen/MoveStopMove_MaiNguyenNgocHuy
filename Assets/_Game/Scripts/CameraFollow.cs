using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Transform target;
    
    public Vector3 offset;

    private void Awake()
    {
        instance = this;
        
        OnInit();
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    public void OnInit()
    {
        offset = Vector3.up * 15 + Vector3.back * 15;
        transform.rotation = Quaternion.Euler(Vector3.right * 45);
    }

    public void EndLevel()
    {
        offset = Vector3.up * 5 + Vector3.forward * 18;
        transform.rotation = Quaternion.Euler(Vector3.up * 180);
    }
}
