using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    public static ParticlePoolManager Instance;

    [System.Serializable]
    public class ParticlePool
    {
        public string poolName;
        public GameObject prefab;
        public int size;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }

    public List<ParticlePool> particlePools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        foreach (var pool in particlePools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                pool.poolQueue.Enqueue(obj);
            }
        }
    }

    public GameObject SpawnParticle(string poolName, Vector3 position, Quaternion rotation)
    {
        ParticlePool pool = particlePools.Find(p => p.poolName == poolName);
        if (pool != null && pool.poolQueue.Count > 0)
        {
            GameObject obj = pool.poolQueue.Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            pool.poolQueue.Enqueue(obj); // Recycle the particle
            return obj;
        }
        return null;
    }
}