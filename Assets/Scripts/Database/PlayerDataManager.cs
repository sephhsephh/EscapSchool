using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance { get { return _instance; } }

    public UserData CurrentUser { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure this scene stays loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Initialize()
    {
        // Any initialization logic needed
        Debug.Log("PlayerDataManager initialized");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unload the Managers scene if it's not needed
        if (scene.name != "Managers" && SceneManager.GetSceneByName("Managers").isLoaded)
        {
            // Keep managers loaded - we need them!
            // SceneManager.UnloadSceneAsync("Managers");
        }
    }

    public void SetUserData(UserData userData)
    {
        CurrentUser = userData;
        Debug.Log($"User data set: {userData.email}, Role: {userData.role}");
    }

    public void ClearUserData()
    {
        CurrentUser = null;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}