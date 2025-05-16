using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostClientUIManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(HostButtonClick);
        clientButton.onClick.AddListener(ClientButtonClick);

        // Make sure network manager persists between scenes
        if (NetworkManager.Singleton != null)
        {
            DontDestroyOnLoad(NetworkManager.Singleton.gameObject);
        }
    }

    private void HostButtonClick()
    {
        StartGame(true);
    }

    private void ClientButtonClick()
    {
        StartGame(false);
    }

    private void StartGame(bool asHost)
    {
        // Updated enum reference
        NetworkConnectionData.ConnectionType = asHost ?
            NetworkConnectionData.NetworkConnectionType.Host :
            NetworkConnectionData.NetworkConnectionType.Client;

        SceneManager.LoadScene("SampleSceneMulti");
    }
}