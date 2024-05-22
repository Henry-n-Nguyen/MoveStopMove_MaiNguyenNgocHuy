using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;
using HuySpace;

public abstract class AbstractCharacter : MonoBehaviour
{
    // Editor
    [Header("SetUp-References")]
    [SerializeField] protected Transform characterTransform;
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
    public int index;

    public int point = 0;

    protected float moveSpeed = 5f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

    // Bool variables
    protected bool isDead;
    protected bool isIngame;
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
        targetScript.enabled = !IsOnPause();
        isReadyToAttack = IsReadyToAttack();

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public virtual void OnInit()
    {
        isDead = false;
        isBoosted = false;

        targetsInRange.Clear();
        boostedType.Clear();

        radarObject.radius = attackRange;

        OnScaleRatioChanges();

        ChangeState(new IdleState());
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

                CameraFollow.instance.offset *= scaleRatio;

                break;
            case 7: 
                scaleRatio = 1.25f; 
                OnScaleRatioChanges();

                CameraFollow.instance.offset *= scaleRatio;

                break;
            case 15: 
                scaleRatio = 1.5f; 
                OnScaleRatioChanges(); 
                
                CameraFollow.instance.offset *= scaleRatio;

                break;
            case 24: 
                scaleRatio = 1.9f; 
                OnScaleRatioChanges(); 
                
                CameraFollow.instance.offset *= scaleRatio;

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
    public bool IsReadyToAttack()
    {
        if (BulletManager.instance.IsBulletActivated(this.index)) { 
            this.weapon.Despawn();
            return false;
        }
        else
        {
            this.weapon.Spawn();
            return true;
        }
    }

    public bool IsOnPause()
    {
        GamePlayState gamePlayState = GamePlayManager.instance.currentGamePlayState;

        return gamePlayState != GamePlayState.Ingame;
    }

    public bool IsOnShop()
    {
        GamePlayState gamePlayState = GamePlayManager.instance.currentGamePlayState;

        return gamePlayState == GamePlayState.Shop;
    }

    public virtual void Moving()
    {
        ChangeAnim(Constant.TRIGGER_RUN);
    }

    public virtual void StopMoving()
    {
        ChangeAnim(Constant.TRIGGER_IDLE);
    }

    public virtual void Attack()
    {
        ChangeAnim(Constant.TRIGGER_ATTACK);

        if (isReadyToAttack)
        {
            BulletManager.instance.Spawn(characterScript);

            if (isBoosted)
            {
                ClearBoost();
            }
        }
    }

    public virtual void Dead()
    {
        ParticleSystem VFX = Instantiate(hitVFX, characterTransform);
        ParticleSystem.ColorOverLifetimeModule VFXcolor = VFX.colorOverLifetime;
        VFXcolor.color = skinMeshRenderer.materials[0].color;

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

    protected void TurnTowardClosestCharacter()
    {
        AbstractCharacter closestEnemy = FindClosestCharacter();

        if (closestEnemy != null)
        {
            characterTransform.forward = (closestEnemy.transform.position - characterTransform.position).normalized;
        }
    }

    // Boost
    public void SpeedUp(float multiply) 
    {
        isBoosted = true;

        boostedType.Add(BoostType.SpeedBoost);

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
        foreach (BoostType type in boostedType)
        {
            switch (type)
            {
                case BoostType.EnormousBoost:
                    scaleRatio = tempScaleRatio;
                    moveSpeed = tempSpeed; 
                    agent.speed = moveSpeed * 0.67f;
                    attackRange = tempAttackRange;
                    
                    break;

                case BoostType.SpeedBoost: 
                    moveSpeed = tempSpeed; 
                    agent.speed = moveSpeed * 0.67f; 
                    
                    break;
            }
        }

        OnScaleRatioChanges();

        isBoosted = false;
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
