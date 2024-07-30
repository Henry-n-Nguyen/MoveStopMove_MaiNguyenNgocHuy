using HuySpace;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : AbstractCharacter
{
    [Space(0.3f)]
    [Header("Player Input")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputAction moveAction;

    private bool isDetectedTarget;
    private bool isDiedBefore = false;

    public override void OnInit()
    {
        base.OnInit();

        LoadDataFromUserData();

        ChangeName("You");

        isDiedBefore = false;

        characterTransform.position = Vector3.zero;

        moveAction = playerInput.actions.FindAction(Constant.INPUT_ACTION_MOVING);

        characterTransform.forward = Vector3.back;

        isDetectedTarget = false;
    }

    public void LoadDataFromUserData()
    {
        UserData data = UserDataManager.Ins.userData;

        EquipmentId equippedWeaponId = data.equippedEquipment[(int)EquipmentType.Weapon];
        Equip(equipmentSODatas.GetSOData(EquipmentType.Weapon).GetData(equippedWeaponId).GetPrefab<Weapon>());

        EquipmentId equippedHatId = data.equippedEquipment[(int)EquipmentType.Hat];
        Equip(equipmentSODatas.GetSOData(EquipmentType.Hat).GetData(equippedHatId).GetPrefab<Hat>());

        EquipmentId equippedSkinId = data.equippedEquipment[(int)EquipmentType.Skin];
        Equip(equipmentSODatas.GetSOData(EquipmentType.Skin).GetData(equippedSkinId).GetPrefab<Skin>());

        EquipmentId equippedPantId = data.equippedEquipment[(int)EquipmentType.Pant];
        Equip(equipmentSODatas.GetSOData(EquipmentType.Pant).GetData(equippedPantId).GetPrefab<Pant>());

        EquipmentId equippedAccessoryId = data.equippedEquipment[(int)EquipmentType.Accessory];
        Equip(equipmentSODatas.GetSOData(EquipmentType.Accessory).GetData(equippedAccessoryId).GetPrefab<Accessory>());
    }

    public override void Moving()
    {
        base.Moving();

        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = Vector3.right * inputVector.x + Vector3.forward * inputVector.y;

        if (moveDirection != Vector3.zero) characterTransform.forward = moveDirection;

        characterTransform.position += moveDirection.normalized * Time.deltaTime * moveSpeed;

        if (Vector2.Distance(inputVector, Vector2.zero) < 0.01f)
        {
            if (IsReadyToAttack())
            {
                if (radarObject.IsAnyTargetInRange)
                {
                    ChangeState(ATTACK_STATE);
                }
                else
                {
                    ChangeState(IDLE_STATE);
                }
            }
            else
            {
                ChangeState(IDLE_STATE);
            }
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();

        if (radarObject.IsAnyTargetInRange)
        {
            if (IsReadyToAttack() && !isDetectedTarget)
            {
                isDetectedTarget = true;

                ChangeState(ATTACK_STATE);
            }
        }

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.01f)
        {
            ChangeState(PATROL_STATE);

            isDetectedTarget = false;
        }
    }

    public override void ReadyToAttack()
    {
        if (!isDetectedTarget)
        {
            base.ReadyToAttack();

            isDetectedTarget = true;
        }

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.1f)
        {
            ChangeState(PATROL_STATE);

            isDetectedTarget = false;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Dead()
    {
        if (!isDead)
        {
            base.Dead();

            GamePlayManager.Ins.ChangeState(GameState.Lose);

            StartCoroutine(DelayToRevive(2f));
        }
    }

    private IEnumerator DelayToRevive(float time)
    {
        yield return new WaitForSeconds(time);

        if (!isDiedBefore)
        {
            isDiedBefore = true;
            LevelManager.Ins.OnRevive();
        }
        else
        {
            LevelManager.Ins.OnLose();
        }
    }

    public override void Dance()
    {
        base.Dance();
    }

    public override void Win()
    {
        base.Win();
    }

    public void Revive()
    {
        isDead = false;
        boxCollider.enabled = true;

        ChangeState(IDLE_STATE);
    }
}
