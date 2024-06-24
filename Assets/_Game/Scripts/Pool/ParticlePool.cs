
using UnityEngine;
using System.Collections.Generic;
using HuySpace;

public static class ParticlePool
{
    const int DEFAULT_POOL_SIZE = 3;

    private static Transform root;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class pPool
    {
        Transform m_sRoot = null;

        //list prefab ready
        List<ParticleSystem> inactive;
        List<ParticleSystem> active;

        // The prefab that we are pooling
        ParticleSystem prefab;

        int index;

        // Constructor
        public pPool(ParticleSystem prefab, int initialQty, Transform parent)
        {
#if UNITY_EDITOR
            if (prefab.main.loop)
            {
                var main = prefab.main;
                main.loop = false;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not Loop");
                Debug.Log(prefab.name + " ~ Fix To Not Loop");
            }

            if (prefab.main.stopAction != ParticleSystemStopAction.None)
            {
                var main = prefab.main;
                main.stopAction = ParticleSystemStopAction.None;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Stop Action None");
                Debug.Log(prefab.name + " ~ Fix To  Stop Action None");
            }   
            
            if (prefab.main.duration > 1)
            {
                var main = prefab.main;
                main.duration = 1;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Duration By 1");
                Debug.Log(prefab.name + " ~ Fix To Duration By 1");
            }
#endif

            m_sRoot = parent;
            this.prefab = prefab;
            inactive = new List<ParticleSystem>(initialQty);

            for (int i = 0; i < initialQty; i++)
            {
                ParticleSystem particle = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                particle.Stop();
                inactive.Add(particle);
            }
        }

        public int Count {
            get { return inactive.Count;}
        }

        // Spawn an object from our pool
        public ParticleSystem Play(Vector3 pos, Quaternion rot)
        {
            index = index + 1 < inactive.Count ? index + 1 : 0;

            ParticleSystem obj = inactive[index];

            if (obj.isPlaying)
            {
                obj = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                obj.Stop();
                inactive.Insert(index, obj);
            }

            obj.transform.SetPositionAndRotation( pos, rot);
            obj.Play();

            return obj;
        }

        public ParticleSystem Attach(Transform root)
        {
            ParticleSystem obj = Play(Vector3.zero, Quaternion.identity);

            obj.transform.SetParent(root);
            obj.transform.localPosition = Vector3.zero;

            return obj;
        }

        public void Collect(ParticleSystem obj)
        {
            if (inactive.Contains(obj)) 
            {
                obj.transform.SetParent(m_sRoot);
            }
        }

        public void CollectAll()
        {
            foreach (ParticleSystem particle in inactive)
            {
                particle.transform.SetParent(m_sRoot);
            }
        }

        public void Release() {
            while(inactive.Count > 0) {
                GameObject.DestroyImmediate(inactive[0]);
                inactive.RemoveAt(0);
            }
            inactive.Clear();
        }
    }

    //--------------------------------------------------------------------------------------------------

    // All of our pools
    static Dictionary<int, pPool> pools = new Dictionary<int, pPool>();

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    static void Init(ParticleSystem prefab = null, ParticleType type = ParticleType.None, int qty = DEFAULT_POOL_SIZE, Transform parent = null)
    {
        if (prefab != null && !pools.ContainsKey((int)type))
        {
            pools[(int)type] = new pPool(prefab, qty, parent);
        }
    }

    static public void Preload(ParticleSystem prefab, ParticleType type, int qty = 1, Transform parent = null)
    {
        Init(prefab, type, qty, parent);
    }

    static public ParticleSystem Play(ParticleType type, Vector3 pos, Quaternion rot)
    {
        return pools[(int) type].Play(pos, rot);
    }

    static public ParticleSystem Attach(ParticleType type, Transform root)
    {
        return pools[(int)type].Attach(root);
    }

    static public void Collect(ParticleType type, ParticleSystem obj)
    {
        if (pools.ContainsKey((int)type))
        {
            pools[(int)type].Collect(obj);
        }
    }

    static public void CollectAll(ParticleType type)
    {
        if (pools.ContainsKey((int)type))
        {
            pools[(int)type].CollectAll();
        }
    }

    static public void Release(ParticleType type)
    {
        if (pools.ContainsKey((int)type))
        {
            pools[(int)type].Release();
            pools.Remove((int)type);
        }
    }
}

[System.Serializable]
public class ParticleAmount
{
    public ParticleSystem prefab;
    public ParticleType type;
    public int amount;
    public Transform root;
}