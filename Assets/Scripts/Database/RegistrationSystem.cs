using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegistrationSystem : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField firstNameInput;
    public InputField lastNameInput;
    public Text messageText;

    public void OnRegisterButtonClicked()
    {
        PHPRequestHandler.Instance.RegisterUser(
            emailInput.text,
            passwordInput.text,
            firstNameInput.text,
            lastNameInput.text,
            (success, message) => {
                messageText.text = message;
                if (success)
                {
                    SceneManager.LoadScene("LoginScene");
                }
            },
            "user",  // Default role
            false    // Default verified status
        );
    }
}