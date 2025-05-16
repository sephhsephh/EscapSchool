using UnityEngine;
using UnityEngine.EventSystems; // Required for checking UI focus

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentInteractable;
    private InteractionUIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<InteractionUIManager>();
    }

    private void Update()
    {
        // Prevent interaction if a UI input field is focused (e.g., typing in TMP_InputField)
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            //Debug.Log("Focused on UI: " + EventSystem.current.currentSelectedGameObject.name);
            return;
        }

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed. Interacting with: " + currentInteractable.name);
            InteractWithObject(currentInteractable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Entered Interactable: " + other.name);
            currentInteractable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Exited Interactable: " + other.name);
            currentInteractable = null;
        }
    }

    private void InteractWithObject(GameObject obj)
    {
        if (obj.TryGetComponent<LessonComputer>(out LessonComputer lesson))
        {
            Debug.Log("Interacting with Lesson Computer");
            lesson.ActivateLesson();
            uiManager.HideInteractionText();
        }
        else if (obj.TryGetComponent<QuizBlackboard>(out QuizBlackboard quiz))
        {
            Debug.Log("Interacting with Quiz Blackboard");
            quiz.StartQuiz();
            uiManager.HideInteractionText();
        }
        else
        {
            Debug.LogWarning("No interactable component found on " + obj.name);
        }
    }
}
