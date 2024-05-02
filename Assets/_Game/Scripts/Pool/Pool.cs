using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pool : MonoBehaviour
{
    public Transform parent;

    public Enemy prefab;

    // List object in pool
    public Queue<Enemy> inactives = new Queue<Enemy>();

    // List object out pool
    public List<Enemy> actives = new List<Enemy>();

    public Pool(Enemy prefab, int initialQty, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;

        inactives = new Queue<Enemy>(initialQty);
        actives = new List<Enemy>();
    }

    // Preload : Khoi tao Pool
    public void Preload(Enemy prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < amount; i++)
        {
            Despawn(Spawn(Vector3.zero, Quaternion.identity));
        }
    }

    // Spawn : Lay phan tu from Pool
    public Enemy Spawn(Vector3 pos, Quaternion rot)
    {
        Enemy unit;

        if (inactives.Count <= 0)
        {
            unit = Instantiate(prefab, pos, rot, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }

        actives.Add(unit);

        unit.transform.position = pos;
        unit.transform.rotation = rot;

        unit.gameObject.SetActive(true);

        return unit;
    }

    // Despawn : Tra lai phan tu
    public void Despawn(Enemy unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            actives.Remove(unit);
            inactives.Enqueue(unit);

            unit.transform.position = Vector3.zero;
            unit.transform.rotation = Quaternion.identity;

            unit.gameObject.SetActive(false);
            unit.OnInit();
        }
    }


    // Collect : thu thap tat ca phan tu ve Pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }


    // Release : Destroy (Clear) tat ca phan tu cua Pool
    public void Release()
    {
        Collect();
        while (inactives.Count > 0)
        {
            Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}