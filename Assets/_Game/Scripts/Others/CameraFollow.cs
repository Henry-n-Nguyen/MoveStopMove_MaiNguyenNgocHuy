using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [field :SerializeField] private Player target = null;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float zoomSpeed;

    private void LateUpdate()
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
        offset = Vector3.up * 19 + Vector3.back * 19;
        cameraTransform.rotation = Quaternion.Euler(Vector3.right * 45);
    }

    public void SetTarget(Player target)
    {
        this.target = target;
        OnInit();
    }

    public void ChangeCameraState()
    {

    }
}
