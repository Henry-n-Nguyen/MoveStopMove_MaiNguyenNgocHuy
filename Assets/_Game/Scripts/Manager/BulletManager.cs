using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    public Dictionary<int, AbstractBullet> activatedBullets = new Dictionary<int, AbstractBullet>();

    public Dictionary<int, AbstractBullet> bulletPrefabs = new Dictionary<int, AbstractBullet>();

    [SerializeField] private Transform holder;
    [SerializeField] private AbstractBullet[] prefabs;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < prefabs.Length; i++)
        {
                bulletPrefabs.Add(prefabs[i].id, prefabs[i]);
        }
    }

    // Spawn Bullet
    public void Spawn(AbstractCharacter character)
    {
        if (!activatedBullets.ContainsKey(character.index))
        {
            AbstractBullet bullet = GetBullet(character);
            bullet.Spawn();
        }
        else
        {
            activatedBullets[character.index].Spawn();
        }
    }

    public void Despawn(AbstractCharacter character)
    {
        if (activatedBullets.ContainsKey(character.index))
        {
            activatedBullets[character.index].Despawn();
        }
    }

    // Check bullet is loaded yet
    public bool IsBulletLoaded(int index)
    {
        return activatedBullets.ContainsKey(index) && activatedBullets[index] != null;
    }

    public bool IsBulletActivated(int index)
    {
        return IsBulletLoaded(index) && activatedBullets[index].gameObject.activeSelf;
    }

    // Get Activated Bullet
    public AbstractBullet GetBullet(AbstractCharacter character)
    {
        AbstractBullet prefab = GetBulletPrefab(character.GetWeaponId());
        AbstractBullet bullet = Instantiate(prefab, holder);

        bullet.owner = character;

        activatedBullets.Add(character.index, bullet);

        return activatedBullets[character.index];
    }

    public AbstractBullet GetBulletPrefab(int id)
    {
        return bulletPrefabs[id];
    }

    public void Realease(AbstractCharacter character)
    {
        if (!activatedBullets.ContainsKey(character.index))
        {
            Destroy(activatedBullets[character.index]);
            activatedBullets.Remove(character.index);
        }
    }
}
