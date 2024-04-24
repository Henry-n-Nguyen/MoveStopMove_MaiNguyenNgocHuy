using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    List<Bullet> activatedBullets = new List<Bullet>();

    public List<Bullet> bulletPrefabs = new List<Bullet>();

    [SerializeField] private Transform holder;
    [SerializeField] private Bullet[] prefabs;

    private void Awake()
    {
        instance = this;

        foreach (Bullet prefab in prefabs)
        {
            bulletPrefabs.Insert(prefab.id, prefab);
        }
    }

    // Spawn Bullet
    public void Spawn(Character character)
    {
        if (!IsBulletLoaded(character.index))
        {
            Bullet bullet = GetBullet(character);
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
    public Bullet GetBullet(Character character)
    {
        Bullet prefab = GetBulletPrefab(character.GetWeaponId());
        Bullet bullet = Instantiate(prefab, holder);

        bullet.owner = character;

        activatedBullets.Insert(character.index, bullet);

        return activatedBullets[character.index];
    }

    public Bullet GetBulletPrefab(int id)
    {
        return bulletPrefabs[id];
    }
}
