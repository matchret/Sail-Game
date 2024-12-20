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


    /// Initializes the particle pools by creating inactive particles at the start.
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

    /// Spawns a particle from the pool at the specified position and rotation.
    public GameObject SpawnParticle(string poolName, Vector3 position, Quaternion rotation)
    {
        ParticlePool pool = particlePools.Find(p => p.poolName == poolName);
        if (pool != null && pool.poolQueue.Count > 0)
        {
            GameObject obj = pool.poolQueue.Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            // Recycle the particle back to the pool
            pool.poolQueue.Enqueue(obj);
            return obj;
        }
        return null;
    }


    /// Removes a particle by disabling its GameObject.
    public void RemoveParticle(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);  // Disable the particle GameObject
            Debug.Log($"Particle {obj.name} removed.");
        }
        else
        {
            Debug.LogWarning("Attempted to remove a null particle.");
        }
    }
}
