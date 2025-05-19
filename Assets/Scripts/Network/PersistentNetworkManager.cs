using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PersistentNetworkManager : MonoBehaviour
{
    private void Awake()
    {
        // Check if there's already a NetworkManager in the scene
        var existingNetworkManagers = Object.FindObjectsByType<NetworkManager>(FindObjectsSortMode.None);
        if (existingNetworkManagers.Length > 1)
        {
            Destroy(gameObject); // Avoid duplicate NetworkManagers
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
