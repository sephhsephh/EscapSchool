using UnityEngine;
using TMPro;
using Unity.Netcode;

public class InteractionUIManager : NetworkBehaviour
{
    public TMP_Text interactionText;

    private bool isVisible = false;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // Disable or hide the UI for non-local players
            interactionText.gameObject.SetActive(false);
            enabled = false;
            return;
        }
    }

    public void ShowInteractionText(string message)
    {
        if (!IsOwner || isVisible) return;

        interactionText.text = message;
        interactionText.gameObject.SetActive(true);
        isVisible = true;
    }

    public void HideInteractionText()
    {
        if (!IsOwner) return;

        interactionText.gameObject.SetActive(false);
        isVisible = false;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}