using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using System.Threading.Tasks;

public class LoginSystem : MonoBehaviour
{

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    public GameObject loginCanvas;
    public GameObject startCanvas;
    public GameObject invalidLoginPanel;

    public void OnLoginButtonClicked()
    {
        PHPRequestHandler.Instance.LoginUser(
            emailInput.text,
            passwordInput.text,
            async (success, message, userData) => {
                messageText.text = message;
                if (success)
                {
                    Debug.Log($"Welcome {userData.first_name} {userData.last_name}!");
                    Debug.Log($"Role: {userData.role}, Verified: {userData.verified}");

                    //// Load appropriate scene based on role
                    //if (userData.role == "admin")
                    //{
                    //    SceneManager.LoadScene("AdminDashboard"); 
                    //}
                    //else
                    //{
                    //    SceneManager.LoadScene("MainMenu");
                    //}
                    loginCanvas.SetActive(false);
                    startCanvas.SetActive(true);

                }
                else
                {
                    invalidLoginPanel.SetActive(true);
                    await Task.Delay(2000);
                    invalidLoginPanel.SetActive(false);
                }
            }
        );
    }

    public void OpenURL()
    {
        Application.OpenURL("https://localhost/website_name_niyo");
    }
}