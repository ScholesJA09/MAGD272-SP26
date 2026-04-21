using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    public Animator transitionAnimator;
    public float waitTime = 1.0f; // This should match your animation length

    private void Awake()
    {
        // Keep this object alive throughout the entire game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(TransitionLogic(sceneName));
    }

    IEnumerator TransitionLogic(string sceneName)
    {
        // 1. Play FadeOut (Goes to Black)
        transitionAnimator.SetTrigger("StartFadeOut");
        yield return new WaitForSeconds(waitTime);

        // 2. Load the scene
        // When this happens, the Animator object survives (DontDestroyOnLoad)
        SceneManager.LoadScene(sceneName);

        // 3. Play FadeIn (Goes to Clear)
        // We call this MANUALLY so it only happens once the scene is ready
        transitionAnimator.SetTrigger("StartFadeIn");
    }
}