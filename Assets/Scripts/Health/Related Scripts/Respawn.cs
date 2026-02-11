using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [Header("When the player dies, does a UI object appear? Drag it here")][Tooltip("Should be a Canvas object.")]
    public GameObject deathScreen;

    [Header("Is there a different game over scene?")][Tooltip("If true, scene needs a SceneController set properly.")]
    public bool gameOverScene = false;

    [Header("Does the game reset over time? Or does death screen have buttons?")][Tooltip("If false, requires a deathScreen with UI buttons.")]
    public bool automaticRespawn;

    [Header("How long does it take to respawn the player?")]
    public float respawnTime;

    [Header("Restart the level when the player dies?")][Tooltip("If false, use CheckPoint prefabs to set checkpoints.")]
    public bool reloadScene = true;

    [Header("Does dying cause time to stop?")]
    public bool stopTime = false;

    List<CheckPoint> checkPoints;

    private void Awake()
    {
        if(!reloadScene)
        {
            checkPoints = new List<CheckPoint>();

            GameObject temp = new GameObject("SpawnPoint");
            temp.transform.position = transform.position;
            temp.AddComponent<CheckPoint>();
            temp.GetComponent<CheckPoint>().active = temp.GetComponent<CheckPoint>().used = true;
            temp.tag = "CheckPoint";
            checkPoints.Add(temp.GetComponent<CheckPoint>());

            foreach (CheckPoint a in FindObjectsOfType<CheckPoint>()) if(a != temp.GetComponent<CheckPoint>()) checkPoints.Add(a);

            if(checkPoints.Count > 1) Debug.Log(checkPoints.Count + " checkpoints found.", gameObject);
            else Debug.LogWarning("No respawn checkpoints found. Adding player spawnpoint as only checkpoint...", gameObject);
        }

        if(respawnTime < 0)
        {
            Debug.LogError("respawnTime cannot be negative! Setting respawnTime = 0...");
            respawnTime = 0;
        }
    }

    public void useRespawn()    // calls on player death
    {
        StartCoroutine(unscaledTimeWait());
    }

    IEnumerator unscaledTimeWait()
    {
        if (automaticRespawn && deathScreen) deathScreen.SetActive(true);
        if (stopTime) Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(respawnTime);

        if (automaticRespawn) respawnPlayer();
        else
        {
            if (deathScreen) deathScreen.SetActive(true);
            else Debug.LogError("Player is unable to restart the game!");
        }
    }

    /// <summary>
    /// If reloadScene = true, reloads current scene. If reloadScene = false, puts player at last active checkpoint.
    /// </summary>
    public void respawnPlayer()
    {
        if (stopTime) Time.timeScale = 1;

        if (gameOverScene) SceneController.sceneController.loadGameObject();
        if (reloadScene)
        {
            Debug.Log("Respawn script reloading scene..." + SceneManager.GetActiveScene().buildIndex.ToString());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 5000);

            // other respawn things
            GetComponent<PlayerHealth>().revive();

            foreach (IMovingPlatform plat in GameObject.FindObjectsOfType<MovingPlatform>()) plat.resetPosition();
            foreach (IMovingPlatform plat in GameObject.FindObjectsOfType<MovingPlatformWithPoints>()) plat.resetPosition();

            if (deathScreen) deathScreen.SetActive(false);

            transform.position = getActiveCheckpoint();
        }
    }

    /// <summary>
    /// Reloads current scene.
    /// </summary>
    public void restartScene()
    {
        if (stopTime) Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quits game. Only works in builds, not Unity window.
    /// </summary>
    public void quitGame()
    {
        Application.Quit();
    }

    public void unactivateCheckpoints()
    {
        foreach (CheckPoint a in checkPoints) a.active = false;
    }

    Vector2 getActiveCheckpoint()
    {
        foreach (CheckPoint a in checkPoints) if (a.active) return a.transform.position;
        Debug.LogError("Error; no active checkpoints found.", gameObject);
        return Vector2.zero;
    }
}
