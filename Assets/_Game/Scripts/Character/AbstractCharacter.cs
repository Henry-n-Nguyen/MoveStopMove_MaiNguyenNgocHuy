using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.AI;
using HuySpace;

public abstract class AbstractCharacter : MonoBehaviour
{
    // Editor
    [SerializeField] protected Transform characterTransform;
    [SerializeField] protected AbstractCharacter characterScript;

    [SerializeField] protected SphereCollider radarObject;
    [SerializeField] protected Transform attackPoint;

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;

    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Transform hatHolder;
    [SerializeField] protected Hat hat;
    [SerializeField] protected SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] protected SkinnedMeshRenderer pantMeshRenderer;

    // Statitics
    public int index;

    protected float moveSpeed = 5f;
    protected float attackRange = 7.5f;
    protected float scaleRatio = 1f;

    // Bool variables
    protected bool isMoving;
    protected bool isDead;
    [SerializeField] protected bool isReadyToAttack;

    // Public variables
    public bool IsDead => isDead;

    // List target
    public List<AbstractCharacter> targetsInRange = new List<AbstractCharacter>();

    // Private variables
    private IState<AbstractCharacter> currentState;

    private string currentAnimName;

    // Function
    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        isReadyToAttack = IsReadyToAttack();

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void OnInit()
    {
        attackRange = 7.5f;
        radarObject.radius = attackRange;
        scaleRatio = 1f;

        ChangeState(new IdleState());
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

    public void Equip(EquipmentType equipmentType, Material material)
    {
        switch (equipmentType)
        {
            case EquipmentType.Pant:
                pantMeshRenderer.material = material;
                break;
            case EquipmentType.Skin:
                skinMeshRenderer.material = material;
                break;
        }
    } // For material equip such as skin, pant, ...


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

        if (isReadyToAttack) BulletManager.instance.Spawn(characterScript);
    }

    public virtual void Dead()
    {
        ChangeAnim(Constant.TRIGGER_DEAD);
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

    public void WarpTo(Vector3 pos)
    {
        agent.Warp(pos);
    }
}
