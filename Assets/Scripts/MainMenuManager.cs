using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject modeSelectionPanel;
    [SerializeField] private GameObject multiplayerPanel;
   


    public void StartGame()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(false); // Hide the entire start panel
        }

       if (modeSelectionPanel != null)
        {
            modeSelectionPanel.SetActive(true); // Show the mode selection panel
        }
    }

    public void ExitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
        
    public void StartSinglePlayer()
    {
        PlayerPrefs.SetInt("GameMode", 0); // 0 for Single Player
        SceneManager.LoadScene("SampleScene");
    }

    public void StartMultiplayer()
    {
        PlayerPrefs.SetInt("GameMode", 1); // 1 for Multiplayer
        
     modeSelectionPanel.SetActive(false); // Hide the mode selection panel
        multiplayerPanel.SetActive(true); // Show the multiplayer panel
    

    }

    public void BackToStart()
    {
        if (modeSelectionPanel != null)
        {
            modeSelectionPanel.SetActive(false); // Hide the mode selection panel
        }

        if (startPanel != null)
        {
            startPanel.SetActive(true); // Show the start panel
        }
    }
    public void BackToSelection()
    {
            multiplayerPanel.SetActive(false); // Hide the mode selection panel
        
    
            modeSelectionPanel.SetActive(true); // Show the start panel
        
    }
}
