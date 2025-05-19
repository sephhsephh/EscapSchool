using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pausepanel;
    public GameObject pauseButton;
    


    public void onPauseButtonClick()
    {
        Debug.Log("clicked pause");
        pausepanel.SetActive(!pausepanel.activeSelf);
    }
}
