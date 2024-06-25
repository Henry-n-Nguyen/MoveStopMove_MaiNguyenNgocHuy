using HuySpace;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Player : AbstractCharacter
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private InputAction moveAction;

    [Header("EquipmentDataSO")]
    [SerializeField] private EquipmentDataSO equipmentDataSO;

    private bool isDetectedTarget;

    public override void OnInit()
    {
        base.OnInit();

        LoadDataFromUserData();

        ChangeName("You");

        characterTransform.position = Vector3.zero;

        moveAction = playerInput.actions.FindAction(Constant.INPUT_ACTION_MOVING);

        Rotate(Direct.Back);

        isDetectedTarget = false;
    }

    public void LoadDataFromUserData()
    {
        UserData data = UserDataManager.instance.userData;

        if (data.isSpecialEquipped)
        {
            int equippedWeaponId = data.equippedWeaponId;
            Equip(EquipmentType.Weapon, equipmentDataSO.GetWeaponById(equippedWeaponId));
            int equippedSpecialId = data.equippedSpecialId;
            Equip(EquipmentType.Special, equipmentDataSO.GetSpecialById(equippedSpecialId));
        }
        else
        {
            int equippedWeaponId = data.equippedWeaponId;
            Equip(EquipmentType.Weapon, equipmentDataSO.GetWeaponById(equippedWeaponId));

            int equippedHatId = data.equippedHatId;
            Equip(EquipmentType.Hat, equipmentDataSO.GetHatById(equippedHatId));

            int equippedPantId = data.equippedPantId;
            Equip(EquipmentType.Pant, equipmentDataSO.GetPantById(equippedPantId));

            int equippedSkinId = data.equippedSkinId;
            Equip(EquipmentType.Skin, equipmentDataSO.GetSkinById(equippedSkinId));
        }
    }

    public override void DeEquipSpecial()
    {
        base.DeEquipSpecial();
    }

    public override void Moving()
    {
        base.Moving();

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        Direct direction = CheckDirection(inputVector);

        Rotate(direction);

        characterTransform.position += (Vector3.right * inputVector.x + Vector3.forward * inputVector.y) * Time.deltaTime * moveSpeed;

        if (Vector2.Distance(inputVector, Vector2.zero) < 0.01f)
        {
            if (isReadyToAttack)
            {
                if (targetsInRange.Count > 0)
                {
                    TurnTowardClosestCharacter();

                    ChangeState(new AttackState());
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

        if (isInShop)
        {
            ChangeState(new DanceState());
        }

        if (targetsInRange.Count > 0)
        {
            TurnTowardClosestCharacter();

            if (isReadyToAttack && !isDetectedTarget)
            {
                isDetectedTarget = true;

                ChangeState(new AttackState());
            }
        }

        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (Vector2.Distance(inputVector, Vector2.zero) > 0.01f)
        {
            ChangeState(new PatrolState());

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
            ChangeState(new PatrolState());

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

            isDead = true;

            GamePlayManager.instance.LoseGame();
        }
    }

    public void Revive()
    {
        isDead = false;

        isReadyToAttack = true;

        targetsInRange.Clear();

        IsInRangeOfAnyCharacter();

        ChangeState(new IdleState());
    }

    private void IsInRangeOfAnyCharacter()
    {
        List<AbstractCharacter> characterList = new List<AbstractCharacter>();
        characterList.AddRange(BotPool.GetActivatedBotList());

        if (characterList.Count > 0)
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                AbstractCharacter foundedTarget = characterList[i];

                float currentDistanceSq = Vector3.SqrMagnitude(characterTransform.position - foundedTarget.characterTransform.position);

                if (currentDistanceSq <= foundedTarget.attackRange)
                {
                    foundedTarget.targetsInRange.Add(this);
                }

                if (currentDistanceSq <= attackRange)
                {
                    targetsInRange.Add(foundedTarget);
                }
            }
        }
    }

    public override void Dancing()
    {
        base.Dancing();

        if (!isInShop)
        {
            ChangeState(new IdleState());
        }
    }

    public override void Win()
    {
        base.Win();
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
}
