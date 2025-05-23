using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class PHPRequestHandler : MonoBehaviour
{
    private static PHPRequestHandler _instance;
    public static PHPRequestHandler Instance { get { return _instance; } }

    private string baseURL = "http://localhost/escape_school/";

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


    public IEnumerator TestConnection()
    {
        // Simple test query
        using (UnityWebRequest www = UnityWebRequest.Get(baseURL + "test_connection.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Connection test failed: " + www.error);
                // Handle retry logic if needed
            }
        }
    }

    public void RegisterUser(string email, string password, string firstName, string lastName,
                           System.Action<bool, string> callback, string role = "user", bool verified = false)
    {
        StartCoroutine(RegisterUserCoroutine(email, password, firstName, lastName, role, verified, callback));
    }

    private IEnumerator RegisterUserCoroutine(string email, string password, string firstName,
                                            string lastName, string role, bool verified,
                                            System.Action<bool, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("firstName", firstName);
        form.AddField("lastName", lastName);
        form.AddField("role", role);
        form.AddField("verified", verified ? 1 : 0);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                PHPResponse response = JsonUtility.FromJson<PHPResponse>(www.downloadHandler.text);
                callback(response.success, response.message);
            }
            else
            {
                callback(false, "Connection error: " + www.error);
            }
        }
    }

    public void LoginUser(string email, string password, System.Action<bool, string, UserData> callback)
    {
        StartCoroutine(LoginUserCoroutine(email, password, callback));
    }

    private IEnumerator LoginUserCoroutine(string email, string password, System.Action<bool, string, UserData> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonString = www.downloadHandler.text;
                    Debug.Log("Received JSON: " + jsonString);

                    // Directly parse to LoginResponse (no wrapper needed)
                    LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonString);

                    if (response != null)
                    {
                        callback(response.success, response.message, response.user);
                    }
                    else
                    {
                        callback(false, "Failed to parse server response", null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON Parse Error: " + e.Message);
                    callback(false, "Error processing server response", null);
                }
            }
            else
            {
                callback(false, "Connection error: " + www.error, null);
            }
        }
    }

    public void GetQuestions(string difficulty, Action<bool, string, List<Question>> callback)
    {
        StartCoroutine(GetQuestionsCoroutine(difficulty, callback));
    }

    private IEnumerator GetQuestionsCoroutine(string difficulty, Action<bool, string, List<Question>> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("difficulty", difficulty);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "get_questions.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    QuestionsResponse response = JsonUtility.FromJson<QuestionsResponse>(www.downloadHandler.text);
                    if (response != null)
                    {
                        callback(response.success, response.message, response.questions);
                    }
                    else
                    {
                        callback(false, "Failed to parse server response", null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON Parse Error: " + e.Message);
                    callback(false, "Error processing server response", null);
                }
            }
            else
            {
                callback(false, "Connection error: " + www.error, null);
            }
        }
    }
}


[System.Serializable]
public class LoginResponseWrapper
{
    public LoginResponse response;
}


[System.Serializable]
public class PHPResponse
{
    public bool success;
    public string message;
}

[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public UserData user;
}
[System.Serializable]
public class UserData
{
    public int id;
    public string email;
    public string first_name;
    public string last_name;
    public string role;
    public bool verified;
}

[System.Serializable]
public class Question
{
    public int id;
    public string question;
    public string answer;
    public string difficulty;
}

[System.Serializable]
public class QuestionsResponse
{
    public bool success;
    public string message;
    public List<Question> questions;
}
