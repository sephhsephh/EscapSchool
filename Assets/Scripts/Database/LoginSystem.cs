using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject errorPanel;

    public void OnLoginButtonClicked()
    {
        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            messageText.text = "Please enter both email and password";
            return;
        }

        messageText.text = "Logging in...";

        PHPRequestHandler.Instance.LoginUser(
            emailInput.text,
            passwordInput.text,
            async (success, message, userData) => {
                messageText.text = message;

                if (success)
                {
                    // Store user data in the manager
                    PlayerDataManager.Instance.SetUserData(userData);
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    errorPanel.SetActive(true);
                    await Task.Delay(2000);
                    errorPanel.SetActive(false);
                }
            }
        );
    }
    public void OnRegisterButtonClicked()
    {
        Application.OpenURL("http://localhost/escape_school/register-test.php");
    }

}