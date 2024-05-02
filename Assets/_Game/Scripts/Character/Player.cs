using HuySpace;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : AbstractCharacter
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private InputAction moveAction;


    private bool isDetectedTarget;

    public override void OnInit()
    {
        base.OnInit();

        isDetectedTarget = false;

        Rotate(Direct.Forward);

        moveAction = playerInput.actions.FindAction(Constant.INPUT_ACTION_MOVING);
    }

    public override void Moving()
    {
        base.Moving();

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        Direct direction = CheckDirection(inputVector);

        Rotate(direction);

        characterTransform.position += (Vector3.right * inputVector.x + Vector3.forward * inputVector.y) * Time.deltaTime * moveSpeed;

        if (Vector2.Distance(inputVector, Vector2.zero) < 0.1f)
        {
            if (isReadyToAttack)
            {
                if (targetsInRange.Count > 0)
                {
                    TurnTowardClosestEnemy();

                    if (isReadyToAttack)
                    {
                        ChangeState(new AttackState());
                    }
                }
                else
                {
                    ChangeState(new IdleState());
                }
            }
            else
            {
                ChangeState(new IdleState());
            }
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();

        if (targetsInRange.Count > 0)
        {
            TurnTowardClosestEnemy();

            if (isReadyToAttack && !isDetectedTarget)
            {
                isDetectedTarget = true;

                ChangeState(new AttackState());
            }
        }

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.1f)
        {
            ChangeState(new PatrolState());

            isDetectedTarget = false;
        }
    }

    public override void Attack()
    {
        if (!isDetectedTarget)
        {
            base.Attack();

            isDetectedTarget = true;
        }

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.1f)
        {
            ChangeState(new PatrolState());

            isDetectedTarget = false;
        }
    }

    private void Rotate(Direct dir)
    {
        switch (dir)
        {
            case Direct.Forward:
                characterTransform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case Direct.ForwardRight:
                characterTransform.rotation = Quaternion.Euler(Vector3.up * 45f);
                break;
            case Direct.Right:
                characterTransform.rotation = Quaternion.Euler(Vector3.up * 90f);
                break;
            case Direct.BackRight:
                characterTransform.rotation = Quaternion.Euler(Vector3.up * 135f);
                break;
            case Direct.Back:
                characterTransform.rotation = Quaternion.Euler(Vector3.up * 180f);
                break;
            case Direct.ForwardLeft:
                characterTransform.rotation = Quaternion.Euler(Vector3.down * 45f);
                break;
            case Direct.Left:
                characterTransform.rotation = Quaternion.Euler(Vector3.down * 90f);
                break;
            case Direct.BackLeft:
                characterTransform.rotation = Quaternion.Euler(Vector3.down * 135f);
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

    private AbstractCharacter FindClosestEnemy()
    {
        AbstractCharacter closestEnemy = null;

        float minDistance = 0f;

        foreach (AbstractCharacter character in targetsInRange)
        {
            float distance = Vector3.Distance(characterTransform.position, character.transform.position);

            if (minDistance < distance)
            {
                minDistance = distance;
                closestEnemy = character;
            }
        }

        return closestEnemy;
    }

    private void TurnTowardClosestEnemy()
    {
        Vector3 closestEnemyPosition = FindClosestEnemy().transform.position;

        characterTransform.forward = (closestEnemyPosition - characterTransform.position).normalized;
    }
}