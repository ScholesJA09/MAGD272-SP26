using UnityEngine; // This fixes the MonoBehaviour error!
using UnityEngine.SceneManagement; // You'll need this for scene loading later

public class GameManager : MonoBehaviour // Changed from PlayerData to match your filename
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth;

    [Header("Scene Management")]
    public string nextEntryPoint;

    private void Awake()
    {
        // This is the Singleton pattern: it ensures only one Manager exists
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
}