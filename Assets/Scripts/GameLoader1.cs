using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameLoader1 : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("SampleSceneMulti", LoadSceneMode.Single);
    }
}
