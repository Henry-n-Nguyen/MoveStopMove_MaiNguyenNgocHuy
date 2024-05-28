using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    private Vector3 offset;

    public Transform target;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 _currentVelocity = Vector3.zero;
    
    private void Awake()
    {
        instance = this;

        OnInit();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    public void OnInit()
    {
        offset = Vector3.up * 18 + Vector3.back * 18;
        cameraTransform.rotation = Quaternion.Euler(Vector3.right * 45);
    }

    public void EndLevel()
    {
        offset = Vector3.up * 5 + Vector3.forward * 18;
        cameraTransform.rotation = Quaternion.Euler(Vector3.up * 180);
    }
}
