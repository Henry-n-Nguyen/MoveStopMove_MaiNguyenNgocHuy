using HuySpace;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveByJoyStick : Character
{
    const string TAG_ENEMY = "Enemy";

    const string INPUT_ACTION_MOVING = "Moving";

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private InputAction moveAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TAG_ENEMY))
        {
            MoveByNavMeshAgent enemy = other.GetComponent<MoveByNavMeshAgent>();
            enemy.IsTargeted(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TAG_ENEMY))
        {
            MoveByNavMeshAgent enemy = other.GetComponent<MoveByNavMeshAgent>();
            enemy.IsTargeted(false);
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        Rotate(Direct.Forward);

        moveAction = playerInput.actions.FindAction(INPUT_ACTION_MOVING);
    }

    public override void Moving()
    {
        base.Moving();

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        Direct direction = CheckDirection(inputVector);

        Rotate(direction);

        playerTransform.position += (Vector3.right * inputVector.x + Vector3.forward * inputVector.y) * Time.deltaTime * moveSpeed;

        if (Vector2.Distance(inputVector, Vector2.zero) < 0.1f) ChangeState(new IdleState());
    }

    public override void StopMoving()
    {
        base.StopMoving();

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.1f) ChangeState(new PatrolState());
    }

    public override void Attack()
    {
        base.Attack();
    }

    private void Rotate(Direct dir)
    {
        switch (dir)
        {
            case Direct.Forward:
                playerTransform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case Direct.ForwardRight:
                playerTransform.rotation = Quaternion.Euler(Vector3.up * 45f);
                break;
            case Direct.Right:
                playerTransform.rotation = Quaternion.Euler(Vector3.up * 90f);
                break;
            case Direct.BackRight:
                playerTransform.rotation = Quaternion.Euler(Vector3.up * 135f);
                break;
            case Direct.Back:
                playerTransform.rotation = Quaternion.Euler(Vector3.up * 180f);
                break;
            case Direct.ForwardLeft:
                playerTransform.rotation = Quaternion.Euler(Vector3.down * 45f);
                break;
            case Direct.Left:
                playerTransform.rotation = Quaternion.Euler(Vector3.down * 90f);
                break;
            case Direct.BackLeft:
                playerTransform.rotation = Quaternion.Euler(Vector3.down * 135f);
                break;
        }
    }

    private Direct CheckDirection(Vector2 direction)
    {
        switch ((int)Mathf.Round(direction.x))
        {
            case 1:
                switch ((int)Mathf.Round(direction.y))
                {
                    case 1: return Direct.ForwardRight;
                    case 0: return Direct.Right;
                    case -1: return Direct.BackRight;
                }
                return Direct.None;
            case 0:
                switch ((int)Mathf.Round(direction.y))
                {
                    case 1: return Direct.Forward;
                    case 0: return Direct.None;
                    case -1: return Direct.Back;
                }
                return Direct.None;
            case -1:
                switch ((int)Mathf.Round(direction.y))
                {
                    case 1: return Direct.ForwardLeft;
                    case 0: return Direct.Left;
                    case -1: return Direct.BackLeft;
                }
                return Direct.None;
        }

        return Direct.None;
    }
}
