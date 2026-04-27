using UnityEngine;

public class TriggerText : MonoBehaviour
{
    [Header("The Canvas or Text to Show")]
    public GameObject textElement;

    private void Awake()
    {
        // Hide the text when the game starts
        if (textElement != null)
            textElement.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textElement.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textElement.SetActive(false);
        }
    }
}