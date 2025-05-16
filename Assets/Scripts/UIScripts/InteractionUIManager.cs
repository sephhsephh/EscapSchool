using UnityEngine;
using TMPro;

public class InteractionUIManager : MonoBehaviour
{
    public TMP_Text interactionText;

    private bool isVisible = false;

    public void ShowInteractionText(string message)
    {
        if (isVisible) return; // Prevent overlapping
        interactionText.text = message;
        interactionText.gameObject.SetActive(true);
        isVisible = true;
    }

    public void HideInteractionText()
    {
        interactionText.gameObject.SetActive(false);
        isVisible = false;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}
