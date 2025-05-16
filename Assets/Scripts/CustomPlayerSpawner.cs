using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class CustomPlayerSpawner : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
    private Vector3 spawnPosition = new(43.5f, -9.215f, -199.8f);
    private Vector3 spawnPosition2 = new(28.5f, -9.215f, -199.8f);

    private bool hasSpawnedHost = false;

    private void Start()
    {
        Debug.Log("CustomPlayerSpawner: Start method called.");

        int gameMode = PlayerPrefs.GetInt("GameMode", -1);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        if (gameMode == 0) // Single Player
        {
            Debug.Log("Starting as Host for Single Player...");
            NetworkManager.Singleton.StartHost();
            SpawnHostPlayer(); // Manually spawn host for singleplayer
        }
        else if (gameMode == 1) // Multiplayer
        {
            StartMultiplayerConnection();
        }
    }

    private void StartMultiplayerConnection()
    {
        switch (NetworkConnectionData.ConnectionType)
        {
            case NetworkConnectionData.NetworkConnectionType.Host:
                NetworkManager.Singleton.StartHost();
                // Host spawn is handled in OnClientConnected
                break;

            case NetworkConnectionData.NetworkConnectionType.Client:
                NetworkManager.Singleton.StartClient();
                break;
        }
    }

    private void SpawnHostPlayer()
    {
        if (!hasSpawnedHost && NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Spawning host player (boy)");
            GameObject playerInstance = Instantiate(boyPrefab, spawnPosition, Quaternion.identity);
            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
            networkObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId, true);
            hasSpawnedHost = true;
            StartCoroutine(AssignCameraAfterSpawn(playerInstance));
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[OnClientConnected] ClientID={clientId} | IsServer={NetworkManager.Singleton.IsServer}");

        // Handle host player spawn for multiplayer
        if (NetworkManager.Singleton.IsServer &&
            clientId == NetworkManager.Singleton.LocalClientId &&
            PlayerPrefs.GetInt("GameMode") == 1) // Multiplayer host
        {
            SpawnHostPlayer();
        }
        // Handle client player spawn
        else if (NetworkManager.Singleton.IsServer &&
                clientId != NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log($"Spawning girl for client {clientId}");
            GameObject playerInstance = Instantiate(girlPrefab, spawnPosition2, Quaternion.identity);
            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
            networkObject.SpawnWithOwnership(clientId, true);
        }

        // Client setup
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            StartCoroutine(SetupLocalPlayerCoroutine());
        }
    }

    private IEnumerator SetupLocalPlayerCoroutine()
    {
        // Wait until player object exists
        while (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null;
        }

        NetworkObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
        Debug.Log($"CLIENT: Found my player: {localPlayer.gameObject.name}");

        if (Camera.main.TryGetComponent<SmoothSideScrollCamera>(out var cameraScript))
        {
            Debug.Log($"Assigning {localPlayer.gameObject.name} to the camera.");
            cameraScript.AssignPlayer(localPlayer.transform);
        }
        else
        {
            Debug.LogError("No SmoothSideScrollCamera script found on the main camera.");
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
        NetworkConnectionData.ConnectionType = NetworkConnectionData.NetworkConnectionType.None;
    }


private IEnumerator AssignCameraAfterSpawn(GameObject playerInstance)
    {
        yield return null;

        if (Camera.main.TryGetComponent<SmoothSideScrollCamera>(out var cameraScript))
        {
            Debug.Log($"Assigning {playerInstance.name} to the camera.");
            cameraScript.AssignPlayer(playerInstance.transform);
        }
        else
        {
            Debug.LogError("No SmoothSideScrollCamera script found on the main camera.");
        }
    }


}