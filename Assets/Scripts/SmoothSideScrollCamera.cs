using UnityEngine;

public class SmoothSideScrollCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10);

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Follow on X, Y, and Z axes
            transform.position = smoothedPosition;
        }
    }

    public void AssignPlayer(Transform newPlayer)
    {
        if (newPlayer == null)
        {
            Debug.LogError("AssignPlayer: newPlayer is null!");
            return;
        }

        playerTransform = newPlayer;
        Debug.Log($"Camera assigned to: {newPlayer.name} at position {newPlayer.position}");
    }
}
