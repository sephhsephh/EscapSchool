using UnityEngine;
using TMPro;

public class LessonComputer : MonoBehaviour
{
    [Header("Lesson Settings")]
    

    [Header("UI References")]
    public GameObject lessonPanel;
    public TMP_Text lessonTextDisplay;

    private void Start()
    {
        if (lessonPanel == null)
        {
            Debug.LogError("Lesson Panel is not assigned in the Inspector.");
        }

        if (lessonTextDisplay == null)
        {
            Debug.LogError("Lesson Text Display (TMP_Text) is not assigned in the Inspector.");
        }

        // Ensure panel is hidden initially
        lessonPanel.SetActive(false);
    }

    public void ActivateLesson()
    {
        

        if (lessonPanel != null && lessonTextDisplay != null)
        {
            lessonPanel.SetActive(true);
            //lessonTextDisplay.text = lessonText;
            Debug.Log("Lesson Panel should now be visible.");
        }
        else
        {
            Debug.LogError("Lesson Panel or Text Display is null.");
        }
    }

    public void CloseLesson()
    {
        if (lessonPanel != null)
        {
            lessonPanel.SetActive(false);
            Debug.Log("Lesson Panel is now hidden.");
        }
    }
}
