using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;
using HuySpace;
using System;
using UnityEngine.UIElements;

public abstract class AbstractCharacter : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] protected CharacterConfigSO configSO;

    // Editor
    [Header("SetUp-References")]
    [SerializeField] protected AbstractCharacter characterScript;
    [SerializeField] protected Target targetScript;

    [SerializeField] protected SphereCollider radarObject;
    [SerializeField] protected Transform attackPoint;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;


    [Header("Equipment-References")]
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Transform hatHolder;
    [SerializeField] protected Hat hat;
    [SerializeField] protected SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] protected SkinnedMeshRenderer pantMeshRenderer;

    [Header("Special-Equipment-References")]
    [SerializeField] protected Transform characterHip;
    [SerializeField] protected Transform characterLeftHand;
    [SerializeField] protected Transform characterNeck;

    [Header("VFX/ParticleDataSO")]
    [SerializeField] protected ParticleDataSO particleDataSO;

    // Statitics
    [Header("Public")]
    [HideInInspector] public NavMeshAgent agent;

    public Transform characterTransform;

    public int index;
    public string characterName = "?";
    public int point = 0;
    public int currentScaleLevel = 0;

    public float moveSpeed = 5f;
    public float attackRange = 7.5f;
    public float scaleRatio = 1f;

    // Bool variables
    protected bool isDead;
    protected bool isOnPause = true;
    protected bool isInCostumeShop;
    [SerializeField] protected bool isReadyToAttack;

    // Public variables
    [HideInInspector] public bool IsDead => isDead;

    // List target
    public List<AbstractCharacter> targetsInRange = new List<AbstractCharacter>();

    private List<GameObject> specialAccessorys = new List<GameObject>();

    // Private variables
    private IState<AbstractCharacter> currentState;

    private string currentAnimName;

    // Boost Variables
    public bool isBoosted;
    public bool isHugeBulletBoosted;

    [HideInInspector] public BoostType boostedType;

    protected float tempScaleRatio;
    protected float tempSpeed;
    protected float tempAttackRange;

    private ParticleSystem boostedAura;

    // Function
    private void Start()
    {
        OnInit();
        SubscribeEvent();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public void SubscribeEvent()
    {
        GamePlayManager.instance.OnUIChanged += IsOnPause;
        GamePlayManager.instance.OnUIChanged += IsInCostumeShop;
    }

    public virtual void OnInit()
    {
        if (isBoosted)
        {
            ClearBoost();
        }

        point = 0;
        currentScaleLevel = 0;

        isReadyToAttack = true;

        isDead = false;

        targetsInRange.Clear();

        scaleRatio = configSO.RawScale;
        moveSpeed = configSO.RawMoveSpeed;
        attackRange = configSO.RawAttackRange;

        OnScaleRatioChanges();

        OnPointChanges();

        radarObject.radius = attackRange;

        ChangeState(new IdleState());
    }

    public void OnScaleRatioChanges()
    {
        characterTransform.localScale = Vector3.one * scaleRatio;
        moveSpeed = configSO.RawMoveSpeed * scaleRatio;
        attackRange = configSO.RawAttackRange * scaleRatio;
    }

    public void OnPointChanges()
    {
        if (currentScaleLevel < configSO.GetMaxScaleLevel()) {
            if (point == configSO.GetLevelMilestone(currentScaleLevel))
            {
                scaleRatio = configSO.GetLevel2Scale(currentScaleLevel);
                currentScaleLevel++;
            }
        }
    }

    public void ChangeState(IState<AbstractCharacter> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            anim.ResetTrigger(animName);
            anim.SetTrigger(currentAnimName);
        }
    }

    public void ChangeName(string value)
    {
        characterName = value;

        targetScript.TargetName = characterName;
    }

    // Overloadded Equip function for each type of Equipment need
    public void Equip(EquipmentType equipmentType, Weapon prefab)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                if (weapon != null) Destroy(weapon.gameObject);
                Weapon equippedWeapon = Instantiate(prefab, weaponHolder);
                weapon = equippedWeapon;
                break;
        }
    } // For weapon equip

    public void Equip(EquipmentType equipmentType, Hat prefab) {
        switch (equipmentType)
        {
            case EquipmentType.Hat:
                if (hat != null) Destroy(hat.gameObject);
                Hat equippedHat = Instantiate(prefab, hatHolder);
                hat = equippedHat;
                break;
        }
    } // For hat equip

    public void Equip(EquipmentType equipmentType, Skin skin)
    {
        switch (equipmentType)
        {
            case EquipmentType.Skin:
                skinMeshRenderer.material = skin.material;
                break;
        }

        targetScript.TargetColor = skinMeshRenderer.materials[0].color;
    } // For skin

    public void Equip(EquipmentType equipmentType, Pant pant)
    {
        switch (equipmentType)
        {
            case EquipmentType.Pant:
                pantMeshRenderer.material = pant.material;
                break;
        }
    } // For pant

    public void Equip(EquipmentType equipmentType, Special special)
    {
        switch (equipmentType)
        {
            case EquipmentType.Special:

                if (special.specialHat != null)
                {
                    Equip(EquipmentType.Hat, special.specialHat);
                }

                if (special.specialPant != null)
                {
                    Equip(EquipmentType.Pant, special.specialPant);
                }

                if (special.specialSkin != null)
                {
                    Equip(EquipmentType.Skin, special.specialSkin);
                }

                if (special.wing != null)
                {
                    GameObject characterWing = Instantiate(special.wing, characterNeck);
                    specialAccessorys.Add(characterWing);
                }

                if (special.tail != null)
                {
                    GameObject characterTail = Instantiate(special.tail, characterHip);
                    specialAccessorys.Add(characterTail);
                }

                if (special.leftHandAccessory != null)
                {
                    GameObject characterLeftAccessory = Instantiate(special.leftHandAccessory, characterLeftHand);
                    specialAccessorys.Add(characterLeftAccessory);
                }

                break;
        }
    } // For special

    public virtual void DeEquipSpecial()
    {
        while (specialAccessorys.Count > 0)
        {
            Destroy(specialAccessorys[0].gameObject);
            specialAccessorys.RemoveAt(0);
        }
    }

    // Public function
    public void IsOnPause()
    {
        GamePlayState gamePlayState = GamePlayManager.instance.currentGamePlayState;

        if (gamePlayState != GamePlayState.Ingame)
        {
            isOnPause = true;
            targetScript.enabled = false;
            ChangeState(new IdleState());
        }
        else
        {
            isOnPause = false;
            targetScript.enabled = true;
        }
    }

    public void IsInCostumeShop()
    {
        GamePlayState gamePlayState = GamePlayManager.instance.currentGamePlayState;

        isInCostumeShop = gamePlayState == GamePlayState.CostumeShop;
    }

    public void IsReadyToAttack()
    {
        if (BulletManager.instance.IsBulletActivated(this.index))
        {
            this.weapon.Despawn();
            isReadyToAttack = false;
        }
        else
        {
            this.weapon.Spawn();
            isReadyToAttack = true;
        }
    }

    // State functions
    public virtual void Moving()
    {
        if (isDead)
        {
            return;
        }

        ChangeAnim(Constant.TRIGGER_RUN);
    }

    public virtual void StopMoving()
    {
        if (isDead)
        {
            return;
        }

        ChangeAnim(Constant.TRIGGER_IDLE);
    }
    
    public virtual void ReadyToAttack()
    {
        if (isDead)
        {
            return;
        }

        if (isReadyToAttack)
        {
            ChangeAnim(Constant.TRIGGER_ATTACK);
        }
    }

    public virtual void Attack()
    {
        BulletManager.instance.Spawn(characterScript);

        if (isBoosted)
        {
            ClearBoost();
        }
    }

    public virtual void Dead()
    {
        ParticleSystem VFX = ParticlePool.Play(ParticleType.HitVFX, characterTransform.position, Quaternion.identity);
        ParticleSystem.ColorOverLifetimeModule VFXcolor = VFX.colorOverLifetime;
        VFXcolor.color = skinMeshRenderer.materials[0].color;

        BulletManager.instance.Despawn(characterScript);

        ChangeAnim(Constant.TRIGGER_DEAD);

        if (isBoosted)
        {
            ClearBoost();
        }
    }

    public virtual void Dancing()
    {
        if (isDead)
        {
            return;
        }

        ChangeAnim(Constant.TRIGGER_DANCE_SHOP);
    }

    public virtual void Win()
    {
        if (isDead)
        {
            return;
        }

        ChangeAnim(Constant.TRIGGER_WIN);
    }

    // Support functions
    protected AbstractCharacter FindClosestCharacter()
    {
        AbstractCharacter closestEnemy = null;

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < targetsInRange.Count; i++)
        {
            AbstractCharacter target = targetsInRange[i];

            if (target.IsDead)
            {
                targetsInRange.RemoveAt(i);
                continue;
            }

            Vector3 targetPosition = target.characterTransform.position;

            float distanceSq = Vector3.SqrMagnitude(characterTransform.position - targetPosition);

            if (distanceSq < minDistance)
            {
                minDistance = distanceSq;
                closestEnemy = target;
            }
        }

        return closestEnemy;
    }

    protected void TurnTowardClosestCharacter()
    {
        AbstractCharacter closestEnemy = FindClosestCharacter();

        if (closestEnemy != null)
        {
            Vector3 targetDir = (closestEnemy.characterTransform.position - characterTransform.position).normalized;
            if (targetDir != Vector3.zero) characterTransform.forward = targetDir;
        }
    }

    // Boost functions
    public void CheckBoost()
    {
        if (isBoosted)
        {
            tempScaleRatio = scaleRatio;

            if (boostedAura == null)
            {
                ParticleSystem aura = ParticlePool.Attach(ParticleType.BoostedVFX, characterTransform);
                boostedAura = aura;
            }
            else
            {
                boostedAura.Play();
            }
        }
        else
        {
            boostedAura.Stop();
        }
    }

    protected void ClearBoost()
    {
        scaleRatio = tempScaleRatio;
        OnScaleRatioChanges();

        isBoosted = false;
        isHugeBulletBoosted = false;
        boostedType = BoostType.None;
        CheckBoost();
    }

    // Getter
    public int GetWeaponId()
    {
        return weapon.id;
    }

    public Transform GetAttackPoint()
    {
        return attackPoint;
    }

    public float GetRawAttackRange()
    {
        return configSO.RawAttackRange;
    }
}
