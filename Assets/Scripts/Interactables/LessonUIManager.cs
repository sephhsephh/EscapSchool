using UnityEngine;

public class LessonUIManager : MonoBehaviour
{
    public GameObject lessonPanel;

    public void CloseLessonPanel()
    {
        if (lessonPanel != null)
        {
            lessonPanel.SetActive(false);
        }
    }
}
