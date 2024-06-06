using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private Vector3 offset;

    public Player target = null;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float zoomSpeed;

    private float distanceToTarget;
    
    private void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        MoveCameraFollowTarget();
    }

    private void MoveCameraFollowTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.characterTransform.position + offset * (target.attackRange / target.RawAttackRange);

            zoomSpeed = target.moveSpeed;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, zoomSpeed * Time.deltaTime);
        }
    }

    public void OnInit()
    {
        offset = Vector3.up * 18 + Vector3.back * 18;
        distanceToTarget = Vector3.SqrMagnitude(offset);
        cameraTransform.rotation = Quaternion.Euler(Vector3.right * 45);
    }

    public void EndLevel()
    {
        offset = Vector3.up * 5 + Vector3.forward * 18;
        cameraTransform.rotation = Quaternion.Euler(Vector3.up * 180);
    }
}
