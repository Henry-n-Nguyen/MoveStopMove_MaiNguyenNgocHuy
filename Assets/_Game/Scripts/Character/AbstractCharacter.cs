using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;
using HuySpace;
using System;

public abstract class AbstractCharacter : MonoBehaviour
{
    // Editor
    [Header("SetUp-References")]
    [SerializeField] protected AbstractCharacter characterScript;
    [SerializeField] protected Target targetScript;

    [SerializeField] protected SphereCollider radarObject;
    [SerializeField] protected Transform attackPoint;

    [SerializeField] protected NavMeshAgent agent;
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

    [Header("VFX")]
    [SerializeField] protected ParticleSystem hitVFX;

    // Statitics
    [Header("Public")]
    public Transform characterTransform;

    public int index;

    public int point = 0;

    protected float moveSpeed = 5f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

    // Bool variables
    protected bool isDead;
    protected bool isOnPause = true;
    protected bool isInShop;
    [SerializeField] protected bool isReadyToAttack;

    // Public variables
    public bool IsDead => isDead;

    // List target
    public List<AbstractCharacter> targetsInRange = new List<AbstractCharacter>();

    private List<GameObject> specialAccessorys = new List<GameObject>();

    // Private variables
    private IState<AbstractCharacter> currentState;

    private string currentAnimName;

    // Boost Variables
    public bool isBoosted;
    public bool isHugeBulletBoosted;

    protected List<BoostType> boostedType = new List<BoostType>();

    protected float tempScaleRatio;
    protected float tempSpeed;
    protected float tempAttackRange;

    // Function
    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public virtual void OnInit()
    {
        if (isBoosted)
        {
            ClearBoost();
        }

        isReadyToAttack = true;

        isDead = false;

        targetsInRange.Clear();

        scaleRatio = 1f;
        moveSpeed = 5f;
        attackRange = 7.5f;

        OnScaleRatioChanges();

        OnPointChange();

        radarObject.radius = attackRange;

        ChangeState(new IdleState());

        GamePlayManager.instance.OnUIChanged += IsOnPause;
        GamePlayManager.instance.OnUIChanged += IsInShop;
    }

    public void OnScaleRatioChanges()
    {
        characterTransform.localScale = Vector3.one * scaleRatio;
        moveSpeed *= scaleRatio;
        attackRange *= scaleRatio;
    }

    public void OnPointChange()
    {
        switch (point)
        {
            case 3: 
                scaleRatio = 1.1f; 
                OnScaleRatioChanges();

                break;
            case 7: 
                scaleRatio = 1.25f; 
                OnScaleRatioChanges();

                break;
            case 15: 
                scaleRatio = 1.5f; 
                OnScaleRatioChanges();

                break;
            case 24: 
                scaleRatio = 1.9f; 
                OnScaleRatioChanges();

                break;
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

    public void IsInShop()
    {
        GamePlayState gamePlayState = GamePlayManager.instance.currentGamePlayState;

        isInShop = gamePlayState == GamePlayState.Shop;
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

    // State Function
    public virtual void Moving()
    {
        ChangeAnim(Constant.TRIGGER_RUN);
    }

    public virtual void StopMoving()
    {
        ChangeAnim(Constant.TRIGGER_IDLE);
    }
    
    public virtual void ReadyToAttack()
    {
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
        ParticleSystem VFX = Instantiate(hitVFX, characterTransform);
        ParticleSystem.ColorOverLifetimeModule VFXcolor = VFX.colorOverLifetime;
        VFXcolor.color = skinMeshRenderer.materials[0].color;

        BulletManager.instance.Despawn(characterScript);

        ChangeAnim(Constant.TRIGGER_DEAD);
    }

    public virtual void Dancing()
    {
        ChangeAnim(Constant.TRIGGER_DANCE_SHOP);
    }

    public virtual void Win()
    {
        ChangeAnim(Constant.TRIGGER_WIN);
    }

    protected AbstractCharacter FindClosestCharacter()
    {
        AbstractCharacter closestEnemy = null;

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < targetsInRange.Count; i++)
        {
            AbstractCharacter target = targetsInRange[i];
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
            characterTransform.forward = (closestEnemy.characterTransform.position - characterTransform.position).normalized;
        }
    }

    // Boost
    public void HugeBulletEnhance(float multiply) 
    {
        isBoosted = true;
        isHugeBulletBoosted = true;

        boostedType.Add(BoostType.HugeBulletBoost);

        tempSpeed = moveSpeed;
        moveSpeed *= multiply;
        agent.speed = moveSpeed * 0.67f;
    }

    public void EnormousEnhance()
    {
        isBoosted = true;

        boostedType.Add(BoostType.EnormousBoost);

        tempScaleRatio = scaleRatio;
        tempSpeed = moveSpeed;
        tempAttackRange = attackRange;
        scaleRatio *= 1.5f;
        OnScaleRatioChanges();
    }

    private void ClearBoost()
    {
        for (int i = 0; i < boostedType.Count; i++)
        {
            switch (boostedType[i])
            {
                case BoostType.EnormousBoost:
                    scaleRatio = tempScaleRatio;
                    moveSpeed = tempSpeed; 
                    agent.speed = moveSpeed * 0.67f;
                    attackRange = tempAttackRange;
                    
                    break;

                case BoostType.HugeBulletBoost: 
                    moveSpeed = tempSpeed; 
                    agent.speed = moveSpeed * 0.67f; 
                    
                    break;
            }
        }

        OnScaleRatioChanges();

        isBoosted = false;
        boostedType.Clear();
    }

    // Getter
    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetScaleParametters()
    {
        return scaleRatio;
    }

    public int GetWeaponId()
    {
        return weapon.id;
    }

    public Transform GetAttackPoint()
    {
        return attackPoint;
    }
}
