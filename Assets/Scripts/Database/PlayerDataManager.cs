using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance { get { return _instance; } }

    public UserData CurrentUser { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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
}