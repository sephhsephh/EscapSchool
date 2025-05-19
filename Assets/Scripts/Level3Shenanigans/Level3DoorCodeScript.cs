using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class Level3DoorCodeScript : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject door;
    public GameObject canvas;
    public Button submitButton;
    public GameObject nextTarget;

    public NetworkDoor networkDoor;

    // Correct answer
    public string correctAnswer = "cout <<";

    void Start()
    {
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void CheckAnswer()
    {
        if (inputField.text.Trim().ToLower() == correctAnswer.ToLower())
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