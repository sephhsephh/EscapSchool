using UnityEngine;

public class Level3DoorCloser : MonoBehaviour
{
    public GameObject Door;
    public Vector3 closedPosition = new Vector3(-41f, 42.1f, 48.5f);
    public Vector3 closedRotation = new Vector3(0f, -90f, 0f); // Euler angles (x, y, z)
    public Vector3 openPosition = new Vector3(-21.8f, 42.1f, 60.3f);
    public Vector3 openRotation = new Vector3(0f, -150f, 0f); // Default rotation when open

    private int touchedPlayerCount = 0;
    private const int RequiredPlayerCount = 1;
    private bool doorClosed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!doorClosed && other.CompareTag("Player"))
        {
            touchedPlayerCount++;
            if (touchedPlayerCount >= RequiredPlayerCount)
            {
                CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        if (Door != null)
        {
            Door.transform.position = openPosition;
            Door.transform.rotation = Quaternion.Euler(openRotation);
            doorClosed = false;
        }
    }

    public void CloseDoor()
    {
        if (Door != null)
        {
            Door.transform.position = closedPosition;
            Door.transform.rotation = Quaternion.Euler(closedRotation);
            doorClosed = true;
        }
    }
}