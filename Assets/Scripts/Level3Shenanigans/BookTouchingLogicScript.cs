using Unity.Netcode;
using UnityEngine;

public class BookTouchingLogic : NetworkBehaviour
{
    public string messageToDisplay;
    public ProjectorTouchingLogic projectorLogic;

    public void Start()
    {
        projectorLogic = FindObjectOfType<ProjectorTouchingLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsClient) return;

        if (other.CompareTag("Player"))
        {
            InteractionUIManager interactionUIManager = other.GetComponent<InteractionUIManager>();
            if (interactionUIManager != null && projectorLogic != null && projectorLogic.isBeingTouched.Value)
            {
                interactionUIManager.ShowInteractionText(messageToDisplay);
            }
        }
    }
}