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
            Vector3 targetPosition = target.characterTransform.position + offset * (target.attackRange / target.GetRawAttackRange());

            zoomSpeed = target.moveSpeed - 1f;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, zoomSpeed * Time.deltaTime);
        }
    }

    public void OnInit()
    {
        offset = Vector3.up * 18 + Vector3.back * 18;
        distanceToTarget = Vector3.SqrMagnitude(offset);
        cameraTransform.rotation = Quaternion.Euler(Vector3.right * 45);
    }
}
