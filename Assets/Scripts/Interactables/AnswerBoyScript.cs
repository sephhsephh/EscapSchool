using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerBoyScript : MonoBehaviour
{
    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;
    public TMP_InputField input4;
    public GameObject door;
    public GameObject canvas;
    public Button submitButton;

    // Set the correct answers (you can also do this from the Inspector)
    public string correctAnswer1 = "cout <<";
    public string correctAnswer2 = "a";
    public string correctAnswer3 = "b";
    public string correctAnswer4 = "a + b";

    void Start()
    {
        // Hook up the button click event
        submitButton.onClick.AddListener(CheckAnswers);
    }

    void CheckAnswers()
    {
        if (
            input1.text == correctAnswer1 &&
            input2.text == correctAnswer2 &&
            input3.text == correctAnswer3 &&
            input4.text == correctAnswer4
        )
        {
            Debug.Log("Correct");
            canvas.SetActive(false);
            door.SetActive(false); // Hide the door
            // TODO: Trigger the next event
        }
        else
        {
            Debug.Log("Incorrect");
        }
    }
}
