using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class InteractableObject : NetworkBehaviour
{
    [SerializeField] private string interactionMessage = "Press E to interact";
    [SerializeField] private GameObject owner;
    [SerializeField] private string playerTag = "Player";

    private bool hasInteracted = false;
    private GameObject currentPlayerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (hasInteracted)
        {
            Debug.Log("Already interacted with this object.");
        }

        // Check if this is the local player and meets ownership requirements
        if (!IsValidInteractor(other) || HasBeenInteracted()) return;

        currentPlayerInRange = other.gameObject;
        TryShowInteraction(currentPlayerInRange);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && currentPlayerInRange == other.gameObject)
        {
            TryHideInteraction(currentPlayerInRange);
            currentPlayerInRange = null;
            GetComponent<LessonComputer>()?.CloseLesson();
            MarkAsInteracted(false); // Reset interaction status when player leaves
        }
    }

    private void Update()
    {
        if (currentPlayerInRange == null) return;
        //if (!IsOwner) return; // Only owner can interact

        // Prevent interaction if UI is focused
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed. Interacting with: " + currentPlayerInRange.name);
            InteractWithObject(currentPlayerInRange);
        }
    }

    private bool IsValidInteractor(Collider playerCollider)
    {
        // Check if this is the local player
        if (!IsLocalPlayera(playerCollider)) return false;

        // Check ownership if required
        if (owner != null)
        {
            string baseOwnerName = owner.name.Replace("(Clone)", "");
            string otherName = playerCollider.gameObject.name.Replace("(Clone)", "");
            return otherName == baseOwnerName;
        }

        return playerCollider.CompareTag(playerTag);
    }

    private bool IsLocalPlayera(Collider playerCollider)
    {
        return playerCollider.TryGetComponent(out NetworkObject netObj) && netObj.IsOwner;
    }

    private void TryShowInteraction(GameObject player)
    {
        var uiManager = player.GetComponentInChildren<InteractionUIManager>(true);
        if (uiManager != null)
        {
            uiManager.ShowInteractionText(interactionMessage);
            Debug.Log($"Showing text to {player.name}: {interactionMessage}");
        }
        else
        {
            Debug.LogWarning($"No InteractionUIManager found on player: {player.name}", player);
        }
    }

    private void TryHideInteraction(GameObject player)
    {
        var uiManager = player.GetComponentInChildren<InteractionUIManager>(true);
        uiManager?.HideInteractionText();
    }

    private void InteractWithObject(GameObject player)
    {
        Debug.Log("pressed interacted");
        if (owner != null)
        {
            Debug.Log("Owner is not null");
            string baseOwnerName = owner.name.Replace("(Clone)", "");
            string otherName = player.gameObject.name.Replace("(Clone)", "");
            if (otherName != baseOwnerName)
            {
                Debug.Log("Owner name does not match");
                return;
            }
        }
        if (HasBeenInteracted()) return;
        Debug.Log("Interacting with object: " + gameObject.name);
        var uiManager = player.GetComponentInChildren<InteractionUIManager>(true);

        if (TryGetComponent<LessonComputer>(out LessonComputer lesson))
        {
            Debug.Log("Interacting with Lesson Computer");
            lesson.ActivateLesson();
            uiManager?.HideInteractionText();
            MarkAsInteracted(true);
        }
        else if (TryGetComponent<QuizBlackboard>(out QuizBlackboard quiz))
        {
            Debug.Log("Interacting with Quiz Blackboard");
            quiz.StartQuiz();
            uiManager?.HideInteractionText();
            MarkAsInteracted(true);
        }
        else
        {
            Debug.LogWarning("No interactable component found on " + gameObject.name);
        }
    }

    public void MarkAsInteracted(bool status)
    {
        hasInteracted = status;
        if (status && currentPlayerInRange != null)
        {
            TryHideInteraction(currentPlayerInRange);
        }
    }

    public bool HasBeenInteracted() => hasInteracted;
}