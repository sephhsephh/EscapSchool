using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string interactionMessage = "Press E to interact";
    [SerializeField] private GameObject owner;
    [SerializeField] private string playerTag = "Player";

    private InteractionUIManager uiManager;
    private bool hasInteracted = false;

    private void Start()
    {
        uiManager = FindObjectOfType<InteractionUIManager>();
        if (uiManager == null)
        {
            Debug.LogWarning("InteractionUIManager not found in scene!", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasInteracted) return;

        // If there's an owner, only show to them (including clone versions)
        if (owner != null)
        {
            string baseOwnerName = owner.name.Replace("(Clone)", "");
            string otherName = other.gameObject.name.Replace("(Clone)", "");

            if (otherName == baseOwnerName)
            {
                ShowInteraction();
            }
            return;
        }

        // No owner - show to all players
        if (other.CompareTag(playerTag))
        {
            ShowInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            HideInteraction();
            GetComponent<LessonComputer>()?.CloseLesson();
        }
    }

    private void ShowInteraction()
    {
        if (uiManager != null)
        {
            uiManager.ShowInteractionText(interactionMessage);
        }
    }

    private void HideInteraction()
    {
        if (uiManager != null)
        {
            uiManager.HideInteractionText();
        }
    }

    public void MarkAsInteracted(bool status)
    {
        hasInteracted = status;
        if (status)
        {
            HideInteraction();
        }
    }

    public bool HasBeenInteracted()
    {
        return hasInteracted;
    }
}