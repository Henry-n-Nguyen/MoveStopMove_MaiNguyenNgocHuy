using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HuySpace;
using UnityEngine.TextCore.Text;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;

public class Enemy : AbstractCharacter
{
    public static EAggressiveState E_AGGRESSIVE_STATE = new EAggressiveState();
    public static EPatrolState E_PATROL_STATE = new EPatrolState();

    [Header("Enemy Config")]
    [SerializeField] private EnemyConfigSOs enemyConfigSOs;
    public EnemyConfigSO currentConfigSO;

    [Header("Pre-Setup")]
    [SerializeField] private GameObject targetedMark;

    [Header("Patroling")]
    [SerializeField] private Vector3 desPoint;
    [SerializeField] private float desPointRange;

    [Header("BotType")]
    [SerializeField] private BotType typeOfBot;
    [SerializeField] private LayerMask characterLayer;

    [Header("Target")]
    public AbstractCharacter target = null;

    // Private variables
    private bool isDetectedTarget;
    public float idlingTimer = 0;
    public float patrolingTimer = 0;

    private Coroutine despawnCoroutine = null;

    // Collide and Trigger
    private void OnTriggerEnter(Collider other)
    {
        TriggerWithPlayerRadar(other);
    }

    private void TriggerWithPlayerRadar(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_RADAR)) return;
        IsTargeted(true);
    }

    private void OnTriggerExit(Collider other)
    {
        EndTriggerWithPlayerRadar(other);
    }

    private void EndTriggerWithPlayerRadar(Collider other)
    {
        if (!other.CompareTag(Constant.TAG_RADAR)) return;
        IsTargeted(false);
    }

    private void IsTargeted(bool isTargeted)
    {
        targetedMark.SetActive(isTargeted);
    }

    // Set-up
    public override void OnInit()
    {
        base.OnInit();

        if (despawnCoroutine != null) StopCoroutine(despawnCoroutine);
        IsTargeted(false);
        isDetectedTarget = false;

        patrolingTimer = 0;
        idlingTimer = 0;

        // Randomize
        Player player = CharacterManager.Ins.player;
        if (player != null) OnPointChange(Random.Range(player.point, player.point + 4));

        ChangeName(nameSOData.GetRandomName());
        RandomTypeOfBot();

        Equip(RandomEquipment<Weapon>(EquipmentType.Weapon));
        Equip(RandomEquipment<Hat>(EquipmentType.Hat));
        Equip(RandomEquipment<Skin>(EquipmentType.Skin));
        Equip(RandomEquipment<Pant>(EquipmentType.Pant));
        Equip(RandomEquipment<Accessory>(EquipmentType.Accessory));

        // Reset speed
        agent.speed = moveSpeed * 0.67f;

        // Change state
        ChangeState(IDLE_STATE);
    }

    // Random
    public T RandomEquipment<T>(EquipmentType type) where T : Equipment
    {
        List<EquipmentData> list = equipmentSODatas.GetSOData(type).GetList();
        int randomId = Random.Range(0, list.Count);
        return list[randomId].GetPrefab<T>();
    }

    private void RandomTypeOfBot() {
        // Random type of this bot
        typeOfBot = (BotType)Random.Range(0, 2);

        currentConfigSO = enemyConfigSOs.GetData(typeOfBot);
    }

    // State Methods
    public override void Moving()
    {
        base.Moving();

        if (radarObject.IsAnyTargetInRange && IsReadyToAttack())
        {
            AbstractCharacter character = radarObject.FindClosestCharacter();

            if (character != null && !character.IsDead)
            {
                ChangeState(ATTACK_STATE);
            }
        }

        // Check input condition
        if (isOnPause)
        {
            idlingTimer = 0;
            ChangeState(IDLE_STATE);
        }

        // Check timer
        patrolingTimer += Time.deltaTime;

        if (patrolingTimer > currentConfigSO.PatrolTime)
        {
            idlingTimer = 0;
            ChangeState(IDLE_STATE);
        }

        // Move
        agent.SetDestination(desPoint);

        // Check Joystick state
        Vector3 distanceToDesPoint = characterTransform.position - desPoint;

        if (distanceToDesPoint.magnitude < 1f)
        {
            idlingTimer = 0;
            ChangeState(IDLE_STATE);
        }
    }

    public void AggressiveChase()
    {
        target = CharacterManager.Ins.GetRandomBot();
        desPoint = target.characterTransform.position;
    }

    public void PatrolAround()
    {
        // create random point in a walkRadius
        Vector3 randomDirection = Random.insideUnitSphere * desPointRange;

        // reference point was created to current tranform and find random point with it
        randomDirection += characterTransform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, desPointRange, 1);

        // return result
        desPoint = LevelManager.Ins.GetRandomPosAroundTarget(characterTransform, desPointRange);
    }

    public override void StopMoving()
    {
        base .StopMoving();

        agent.SetDestination(characterTransform.position);

        // Check input condition
        if (isOnPause)
        {
            return;
        }

        // Check Enemy Around
        //if (radarObject.IsAnyTargetInRange && IsReadyToAttack())
        //{
        //    AbstractCharacter character = radarObject.FindClosestCharacter();

        //    if (character != null && !character.IsDead)
        //    {
        //        ChangeState(ATTACK_STATE);
        //    }
        //}

        // Check timer
        idlingTimer += Time.deltaTime;

        if (idlingTimer > currentConfigSO.IdleTime)
        {
            patrolingTimer = 0;

            switch (typeOfBot)
            {
                case BotType.PatrolBot: ChangeState(E_PATROL_STATE); break;
                case BotType.AggressiveBot: ChangeState(E_AGGRESSIVE_STATE); break;
            }
        }
    }

    public override void ReadyToAttack()
    {
        if (isDead)
        {
            return;
        }

        if (radarObject.nearestTarget == null)
        {
            idlingTimer = 0;
            isDetectedTarget = false;

            ChangeState(IDLE_STATE);
        }

        if (!isDetectedTarget)
        {
            agent.SetDestination(characterTransform.position);

            if (radarObject.nearestTarget != null)
            {
                TurnTowardToTarget(radarObject.nearestTarget);
                ChangeAnim(Constant.ANIM_ATTACK);
            }

            isDetectedTarget = true;
        }
    }

    public override void Attack()
    {
        base.Attack();

        isDetectedTarget = false;
        patrolingTimer = 0;

        ChangeState(PATROL_STATE);
    }

    public override void Dead()
    {
        if (!isDead)
        {
            agent.SetDestination(characterTransform.position);

            base.Dead();

            CharacterManager.Ins.DeathBot(this);

            despawnCoroutine = StartCoroutine(DespawnEnemyAfterTime(3f));
        }
    }

    private IEnumerator DespawnEnemyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        CharacterManager.Ins.SpawnBotWithQty(1);

        SimplePool.Despawn(this);
    }
}
