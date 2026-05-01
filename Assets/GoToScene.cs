using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class SceneChanger : MonoBehaviour
{
    [Header("Destination Settings")]
    public string sceneToLoad;
    public string spawnPointName;

    [Header("Gate Settings")]
    public bool requiresKeys = false; // Check this ONLY for the boss door

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. If this is a locked door, check the CollectibleManager
            if (requiresKeys)
            {
                CollectibleManager manager = other.GetComponent<CollectibleManager>();

                // If goal isn't met, block the transition
                if (manager != null && !manager.completed)
                {
                    Debug.Log("Door is locked! Collect all keys (coins) first.");
                    return;
                }
            }

            // 2. Proceed to load scene
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadNextScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find(spawnPointName);
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            Debug.Log("Player moved to spawn point: " + spawnPoint.name);
        }

        if (vcam != null && player != null)
        {
            vcam.Follow = player.transform;
        }

        // Unsubscribe to prevent memory leaks or double-firing
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}