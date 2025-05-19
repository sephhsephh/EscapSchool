using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ProjectorAnswerScript : NetworkBehaviour
{
    public GameObject board;
    public string correctAnswer = "answer here";
    public TMP_InputField answer;
    public Button submitButton;
    public GameObject canvas;
    private void Start()
    {
        submitButton.onClick.AddListener(CheckAnswer);
    }

    public void CheckAnswer()
    {
        if (answer.text.ToLower().Trim() == correctAnswer.ToLower().Trim())
        {
            if (IsServer)
            {
                EnableInteractableClientRpc();
            }
            else
            {
                EnableInteractableServerRpc();
            }
            canvas.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnableInteractableServerRpc()
    {
        EnableInteractableClientRpc();
    }

    [ClientRpc]
    private void EnableInteractableClientRpc()
    {
        if (board.TryGetComponent<InteractableObject>(out var interactable))
        {
            interactable.enabled = true;
        }
    }
}