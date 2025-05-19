using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProfileManager : MonoBehaviour
{
    public TMP_Text fullNameText;
    public TMP_Text emailText;
    public TMP_Text roleText;
    public TMP_Text id;
    
    void Start()
    {
        UserData currentUser = PlayerDataManager.Instance.CurrentUser;

        if (currentUser != null)
        {
            fullNameText.text = $"{currentUser.first_name} {currentUser.last_name}";
            emailText.text = currentUser.email;
            roleText.text = currentUser.role;

            // You could also fetch additional data here
            // PHPRequestHandler.Instance.GetPlayerStats(currentUser.id, OnStatsReceived);
        }
        else
        {
            SceneManager.LoadScene("Login");
        }
    }

    
}
