using UnityEngine;

public class IntroTutorial : MonoBehaviour
{
    void Start()
    {
        Invoke("Deactivate", 10f);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}