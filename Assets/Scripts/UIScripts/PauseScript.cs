using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

public class PauseScript : MonoBehaviour
{
    public GameObject pausepanel;
    public GameObject pauseButton;

    public void onPauseButtonClick()
    {
        Debug.Log("clicked pause");
        pausepanel.SetActive(true);
    }

    public void onResumeButtonClick()
    {
        Debug.Log("clicked resume");
        pausepanel.SetActive(false);
    }

    public void onQuitButtonClick()
    {
        Debug.Log("clicked quit");
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual name of your main menu scene
    }
}
