using System.Collections;
using UnityEngine;

public class RoomTitleUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    public void ShowTitle(string roomName)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeSequence(roomName));
    }

    IEnumerator FadeSequence(string roomName)
    {
        TMPro.TextMeshProUGUI text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = roomName;

        // Fade In
        yield return StartCoroutine(Fade(0, 1));

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}