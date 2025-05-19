using TMPro;
using Unity.Netcode;
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
    public GameObject nextTarget;


    public NetworkDoor networkDoor;

    // Correct answers
    public string correctAnswer1 = "cout <<";
    public string correctAnswer2 = "a";
    public string correctAnswer3 = "b";
    public string correctAnswer4 = "a + b";

    void Start()
    {
        submitButton.onClick.AddListener(CheckAnswers);
    }

    void CheckAnswers()
    {
        if (
            input1.text.Trim().ToLower() == correctAnswer1.ToLower() &&
            input2.text.Trim().ToLower() == correctAnswer2.ToLower() &&
            input3.text.Trim().ToLower() == correctAnswer3.ToLower() &&
            input4.text.Trim().ToLower() == correctAnswer4.ToLower()
        )
        {
            Debug.Log("Correct");
            canvas.SetActive(false);
            door.SetActive(false);

            DisableDoorServerRpc();

            // Broadcast to ALL player prefabs in scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                ArrowPointer pointer = player.GetComponentInChildren<ArrowPointer>(true);
                if (pointer != null)
                {
                    pointer.SetVisible(true);
                    pointer.SetTarget(nextTarget.transform);
                }
                else
                {
                    Debug.LogWarning("ArrowPointer not found on " + player.name);
                }
            }
        }
        else
        {
            Debug.Log("Incorrect");
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void DisableDoorServerRpc()
    {
        // Server validates & disables door
        networkDoor.SetDoorState(false);
    }
}
