
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// basic scene controller
public class SceneController : MonoBehaviour
{

    public static SceneController sceneController;

    public int currentSceneIndex;

    [Header("What is the name of the Game Over scene?")]
    public string GameOverScene;
    [Header("Which is the name of the Main Menu scene?")]
    public string MainMenuSceneName;

    private void Awake()
    {
        if(sceneController == null)
        {
            sceneController = this;
        } else
        {
            Debug.Log("scene manager alreadt existis");
            Destroy(gameObject);
        }

        // grabs the current scene's index
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.sceneLoaded += LevelWasLoaded;
    }

    // feed the desired scene index to load
    public void loadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // feed the desired scene name (a string -- use QUOTES) to load
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // feed the desired scene name
    public void loadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.buildIndex);
    }

    // adds 1 to the current scene index and loads it
    public void loadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // reload the current scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    // scene type version
    public void delayedSceneLoad(Scene scene, float delay)
    {
        StartCoroutine(delayLoad(scene, delay));
    }

    // scene name version
    public void delayedSceneLoad(string scene, float delay)
    {
        StartCoroutine(delayLoad(scene, delay));
    }

    // scene type version
    IEnumerator delayLoad(Scene scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        loadScene(scene.buildIndex);
    }

    // scene name version
    IEnumerator delayLoad(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        loadScene(scene);
    }

    public void delayedNextLoad(float delay)
    {
        Invoke("loadNextScene", delay);
    }

    public void delayedReload(float delay)
    {
        Invoke("ReloadScene", delay);
    }

    //public void OnLevelWasLoaded(int level)
    //{
    //    currentSceneIndex = level;
    //}

    public void LevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
    }

    public void loadGameObject()
    {
        SceneManager.LoadScene(GameOverScene.ToString());
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName.ToString());
    }

    // Quits the game
    public void quitGame()
    {
        Application.Quit();
    }
}