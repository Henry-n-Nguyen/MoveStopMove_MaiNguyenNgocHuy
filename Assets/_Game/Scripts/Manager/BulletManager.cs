using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    List<Bullet> activatedBullets = new List<Bullet>();

    public List<Bullet> bulletPrefabs = new List<Bullet>(); 

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
        if (!character.IsLoadedBullet())
        {
            Bullet bullet = GetBullet(character);
        }
        else
        {
            activatedBullets[character.index].Spawn();
        }
    }

    // Get Activated Bullet
    public Bullet GetBullet(Character character)
    {
        Bullet prefab = GetUIPrefab(character.GetWeaponId());
        Bullet bullet = Instantiate(prefab, character.GetAttackPoint());

        activatedBullets.Insert(character.index, bullet);

        return activatedBullets[character.index];
    }

    public Bullet GetUIPrefab(int id)
    {
        return bulletPrefabs[id];
    }
}
