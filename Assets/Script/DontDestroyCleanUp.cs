using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyCleanup : MonoBehaviour
{
    void Awake()
    { 
        // Find all root objects in the DontDestroyOnLoad scene
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            // Check if object is marked DontDestroyOnLoad
            if (obj != gameObject && obj.scene.name == "DontDestroyOnLoad")
            {
                Debug.Log($"Forcefully destroying: {obj.name}");
                Destroy(obj);
            }
        }
    }

    void Start()
    {
        // Additional cleanup method
        CleanupPersistentObjects(); 
    }

    void CleanupPersistentObjects()
    { 
        // More comprehensive cleanup
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject && obj.scene.name == "DontDestroyOnLoad")
            {
                try
                {
                    DestroyImmediate(obj);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error destroying {obj.name}: {e.Message}");
                }
            }
        }
     }
}