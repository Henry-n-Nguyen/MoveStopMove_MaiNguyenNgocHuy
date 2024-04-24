using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    List<AbstractBullet> activatedBullets = new List<AbstractBullet>();

    public List<AbstractBullet> bulletPrefabs = new List<AbstractBullet>();

    [SerializeField] private Transform holder;
    [SerializeField] private AbstractBullet[] prefabs;

    private void Awake()
    {
        instance = this;

        foreach (AbstractBullet prefab in prefabs)
        {
            bulletPrefabs.Insert(prefab.id, prefab);
        }
    }

    // Spawn Bullet
    public void Spawn(Character character)
    {
        if (!IsBulletLoaded(character.index))
        {
            AbstractBullet bullet = GetBullet(character);
            bullet.Spawn();
        }
        else
        {
            activatedBullets[character.index].Spawn();
        }
    }

    // Check bullet is loaded yet
    public bool IsBulletLoaded(int index)
    {
        try
        {
            return activatedBullets[index] && activatedBullets[index] != null;
        }
        catch
        {
            return false;
        }
    }

    public bool IsBulletActivated(int index)
    {
        try
        {
            return IsBulletLoaded(index) && activatedBullets[index].gameObject.activeSelf;
        }
        catch
        {
            return false;
        }
    }

    // Get Activated Bullet
    public AbstractBullet GetBullet(Character character)
    {
        AbstractBullet prefab = GetBulletPrefab(character.GetWeaponId());
        AbstractBullet bullet = Instantiate(prefab, holder);

        bullet.owner = character;

        activatedBullets.Insert(character.index, bullet);

        return activatedBullets[character.index];
    }

    public AbstractBullet GetBulletPrefab(int id)
    {
        return bulletPrefabs[id];
    }
}
