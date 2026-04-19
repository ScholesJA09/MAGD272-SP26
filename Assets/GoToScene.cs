using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; // Use 'using Cinemachine;' if you are on an older version of Unity

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing hitting the box is the Player
        if (other.CompareTag("Player"))
        {
            // 1. Tell Unity: "When the next scene finishes loading, run the 'OnSceneLoaded' function"
            SceneManager.sceneLoaded += OnSceneLoaded;

            // 2. Load the scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // This part runs ONLY after the new scene is ready
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Locate the objects in the new scene
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();

        // 2. CHECK: If we found both the player and the spawn point...
        if (player != null && spawnPoint != null)
        {
            // --- PUT THE NEW LINE HERE ---
            player.transform.position = spawnPoint.transform.position;
            // -----------------------------

            Debug.Log("Player moved to: " + spawnPoint.name);
        }

        // 3. Set up the camera
        if (vcam != null && player != null)
        {
            vcam.Follow = player.transform;
        }

        // Always unsubscribe at the end
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}