using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Start with the panel fully black
        Color startColor = fadePanel.color;
        startColor.a = 1f;
        fadePanel.color = startColor;

        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            // Lerp the alpha from 1 to 0
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadePanel.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is exactly 0 at the end
        color.a = 0f;
        fadePanel.color = color;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            // Lerp the alpha from 0 to 1
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is exactly 1 at the end
        color.a = 1f;
        fadePanel.color = color;
    }
}
