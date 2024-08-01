using System.Collections.Generic;
using UnityEngine;
using HuySpace;
using UnityEngine.AI;
using System.Collections;

public abstract class AbstractCharacter : GameUnit
{
    // State
    public static IdleState IDLE_STATE = new IdleState();
    public static AttackState ATTACK_STATE = new AttackState();
    public static PatrolState PATROL_STATE = new PatrolState();
    public static DeadState DEAD_STATE = new DeadState();
    public static DeadState DANCE_STATE = new DeadState();

    // Inspector
    [Header("ConfigSO")]
    [SerializeField] protected CharacterConfigSO configSO;

    [Header("EquipmentSODatas")]
    [SerializeField] protected EquipmentSODatas equipmentSODatas;

    [Header("NameSOData")]
    [SerializeField] protected NameSOData nameSOData;

    [Header("SetUp-References")]
    [SerializeField] protected AbstractCharacter characterScript;
    [SerializeField] protected Target targetIndicatorScript;
    [Space(0.3f)]
    [SerializeField] protected Radar radarObject;
    [SerializeField] protected Transform attackPoint;
    [Space(0.3f)]
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected BoxCollider boxCollider;

    [Header("Equipment-References")]
    [SerializeField] protected Transform weaponSlot;
    [SerializeField] protected Weapon weapon;
    [Space(0.3f)]
    [SerializeField] protected Transform accessorySlot;
    [SerializeField] protected Accessory accessory;
    [Space(0.3f)]
    [SerializeField] protected Transform hatSlot;
    [SerializeField] protected Hat hat;
    [Space(0.3f)]
    [SerializeField] protected SkinnedMeshRenderer pantSlot;
    [Space(0.3f)]
    [SerializeField] protected SkinnedMeshRenderer skinSlot;

    // Statitics
    [Header("Self-References")]
    public Transform characterTransform;

    [Header("Info")]
    public int index;
    public string characterName = "?";
    public int point = 0;
    public int currentScaleLevel = 0;

    [Space(0.3f)]
    public float moveSpeed;
    public float attackRange;
    public float scaleRatio;

    // Bool variables
    protected bool isDead;
    protected bool isOnPause = true;

    protected bool weaponIsTemporary = false;
    [SerializeField] protected Weapon tempWeapon;

    [SerializeField] protected bool isReadyToAttack;

    // Public variables
    public bool IsDead => isDead;

    // List target
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

    private VFX boostedAura;

    private Coroutine attackCooldownCoroutine;

    // Function
    private void Start()
    {
        SubscribeEvent();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    private void SubscribeEvent()
    {
        GamePlayManager.Ins.OnUIChangedAction += IsOnPause;
    }

    public virtual void OnInit()
    {
        if (isBoosted)
        {
            ClearBoost();
        }

        currentScaleLevel = 0;

        boxCollider.enabled = true;

        isDead = false;

        radarObject.ClearTargetList();

        OnScaleRatioChange(configSO.RawScale);

        OnPointChange(0);

        ChangeState(IDLE_STATE);
    }

    public void OnScaleRatioChange(float scaleRatio)
    {
        this.scaleRatio = scaleRatio;

        characterTransform.localScale = Vector3.one * scaleRatio;
        moveSpeed = configSO.RawMoveSpeed * scaleRatio;
        attackRange = configSO.RawAttackRange * scaleRatio;
    }

    public void OnPointChange(int point)
    {
        this.point = point;

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
        currentState?.OnExit(this);

        currentState = state;

        currentState?.OnEnter(this);
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
        targetIndicatorScript.TargetName = characterName;
    }

    // Overloadded Equip function for each type of Equipment need
    public void Equip(Weapon prefab)
    {
        if (prefab == null) return;

        if (weapon != null) Destroy(weapon.gameObject);
        
        weapon = Instantiate(prefab, weaponSlot);   
    } // For weapon equip

    public void Equip(Hat prefab) 
    {
        if (prefab == null) return; 
        if (hat != null) Destroy(hat.gameObject);
        hat = Instantiate(prefab, hatSlot);
    } // For hat equip

    public void Equip(Accessory prefab)
    {
        if (prefab == null) return;
        if (accessory != null) Destroy(accessory.gameObject);
        accessory = Instantiate(prefab, accessorySlot);
    } // For accessory equip

    public void Equip(Pant prefab)
    {
        if (prefab == null) return;
        pantSlot.material = prefab.materialColor;
    } // For pant equip

    public void Equip(Skin prefab)
    {
        if (prefab == null) return;
        skinSlot.material = prefab.materialColor;

        targetIndicatorScript.TargetColor = skinSlot.material.color;
    } // For skin equip

    // Public function
    public void IsOnPause()
    {
        if (GamePlayManager.IsState(GameState.Ingame))
        {
            isOnPause = false;
            targetIndicatorScript.enabled = true;
        }
        else
        {
            isOnPause = true;
            targetIndicatorScript.enabled = false;
            ChangeState(IDLE_STATE);
        }
    }

    public bool IsReadyToAttack()
    {
        return weapon.IsActive;
    }

    public void PickUpWeapon(Weapon prefab)
    {
        if (attackCooldownCoroutine != null) StopCoroutine(attackCooldownCoroutine);
        weaponIsTemporary = true;
        if (tempWeapon == null) tempWeapon = Instantiate(weapon, weaponSlot);
        Equip(prefab);
    }

    // State functions
    public virtual void Moving()
    {
        if (isDead)
        {
            return;
        }

        ChangeAnim(Constant.ANIM_RUN);
    }

    public virtual void StopMoving()
    {
        if (isDead) {
            return;
            }

        ChangeAnim(Constant.ANIM_IDLE);
    }
    
    public virtual void ReadyToAttack()
    {
        if (isDead)
        {
            return;
        }

        if (IsReadyToAttack())
        {
            AbstractCharacter character = radarObject.FindClosestCharacter();
            TurnTowardToTarget(character);
            ChangeAnim(Constant.ANIM_ATTACK);
        }
    }

    public virtual void Attack()
    {
        // Spawn Bullet
        weapon.Despawn();
        AxeBullet createdbullet = SimplePool.Spawn<AxeBullet>((PoolType)weapon.id, attackPoint.position, Quaternion.identity);
        createdbullet.SetOwner(this);

        // Axe respawn each 2.5s
        attackCooldownCoroutine = StartCoroutine(AttackCoolDown(2.5f));

        if (isBoosted)
        {
            ClearBoost();
        }
    }

    private IEnumerator AttackCoolDown(float time)
    {
        yield return new WaitForSeconds(time);

        if (!weaponIsTemporary) weapon.Spawn();
        else
        {
            weaponIsTemporary = false;
            Equip(tempWeapon);
            Destroy(tempWeapon.gameObject);
            weapon.Spawn();
        }
    }

    public virtual void Dead()
    {
        isDead = true;
        boxCollider.enabled = false;

        ChangeAnim(Constant.ANIM_DEATH);

        if (isBoosted)
        {
            ClearBoost();
        }
    }

    public virtual void Dance()
    {
        ChangeAnim(Constant.ANIM_DANCE_SHOP);
    }

    public virtual void Win()
    {
        ChangeAnim(Constant.ANIM_WIN);
    }

    // Support functions
    protected void TurnTowardToTarget(AbstractCharacter target)
    {
        if (target == null || target.IsDead) return;

        Vector3 targetPos = target.characterTransform.position;
        characterTransform.LookAt(targetPos + (characterTransform.position.y - targetPos.y) * Vector3.up);
    }

    // Boost functions
    public void CheckBoost()
    {
        if (isBoosted)
        {
            tempScaleRatio = scaleRatio;

            if (boostedAura == null)
            {
                boostedAura = SimplePool.Spawn<VFX>(PoolType.Particle_BoostedVFX, characterTransform);
            }
            else
            {
                boostedAura.Spawn();
            }
        }
        else
        {
            boostedAura.Despawn();
        }
    }

    protected void ClearBoost()
    {
        OnScaleRatioChange(tempScaleRatio);

        isBoosted = false;
        isHugeBulletBoosted = false;
        boostedType = BoostType.None;
        CheckBoost();
    }

    // Getter
    public float GetRawAttackRange()
    {
        return configSO.RawAttackRange;
    }

    public Radar GetRadarObject()
    {
        return radarObject;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
