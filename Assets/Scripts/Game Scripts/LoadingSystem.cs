using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private float fakeLoadDelay = 1f;
    [SerializeField] private Button playbtn;

    [Header("Dependencies")]
    [SerializeField] private PHPRequestHandler phpRequestHandler;
    [SerializeField] private PlayerDataManager playerDataManager;

    private void Start()
    {
        if (playbtn != null) playbtn.gameObject.SetActive(false); // hide it initially
        StartCoroutine(InitializeGameSystems());
    }

    private IEnumerator InitializeGameSystems()
    {
        // Initial fake load (gives time for loading screen to appear)
        yield return new WaitForSeconds(0.1f);
        UpdateLoadingUI("Starting systems...", 0.15F);

        // 1. Initialize PlayerDataManager
        if (playerDataManager != null)
        {
            playerDataManager.Initialize();
            UpdateLoadingUI("Loading player data system...", 0.25F);
            yield return new WaitForSeconds(fakeLoadDelay);
        }

        // 2. Initialize Database Connection
        if (phpRequestHandler != null)
        {
            yield return StartCoroutine(phpRequestHandler.TestConnection());
            UpdateLoadingUI("Connecting to database...", .50f);
        }

        // 3. Load other systems here
        // yield return StartCoroutine(OtherSystem.Instance.Initialize());
        UpdateLoadingUI("Finalizing setup...", .89f);
        yield return new WaitForSeconds(fakeLoadDelay);

        // Complete loading
        UpdateLoadingUI("Ready!", 1f);
        playbtn.gameObject.SetActive(true);
    }
    public void onPlayButtonClick()
    {
        SceneManager.LoadScene("Login");

    }

    private void UpdateLoadingUI(string message, float progress)
    {
        if (loadingText != null) loadingText.text = message;
        if (progressBar != null) progressBar.value = progress;
    }
}