using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BotPool
{
    public static Pool pool;

    public static void Preload(Enemy prefab, int amount, Transform parent = null)
    {
        if (prefab != null)
        {
            pool = new Pool(prefab, amount, parent);
        }
    }

    public static T Spawn<T>(Vector3 pos, Quaternion rot) where T : Enemy
    {
        return pool.Spawn(pos, rot) as T;           
    }

    public static void Despawn(Enemy gameUnit)
    {
        pool.Despawn(gameUnit);
    }

    public static void Collect()
    {
        pool.Collect();
    }

    public static List<Enemy> GetActivatedBotList()
    {
        return pool.actives;
    }
}
