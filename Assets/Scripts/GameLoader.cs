using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
