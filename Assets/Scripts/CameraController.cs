using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraPoint
    {
        public Transform point;
        public float waitTime = 2f;
    }

    public CameraPoint[] cameraPoints;
    private int currentPointIndex = 0;
    private FadeManager fadeManager;

    private void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();

        if (fadeManager == null)
        {
            Debug.LogError("FadeManager not found in the scene.");
            return;
        }

        StartCoroutine(CycleThroughRooms());
    }

    private IEnumerator CycleThroughRooms()
    {
        while (true)
        {
            // Get the current camera point
            Transform targetPoint = cameraPoints[currentPointIndex].point;
            float waitTime = cameraPoints[currentPointIndex].waitTime;

            // Fade Out
            yield return StartCoroutine(fadeManager.FadeOut());

            // Move camera instantly to the target point
            transform.position = targetPoint.position;
            transform.rotation = targetPoint.rotation;

            // Fade In
            yield return StartCoroutine(fadeManager.FadeIn());

            // Wait for a while at this point
            yield return new WaitForSeconds(waitTime);

            // Move to the next point
            currentPointIndex = (currentPointIndex + 1) % cameraPoints.Length;
        }
    }
}
