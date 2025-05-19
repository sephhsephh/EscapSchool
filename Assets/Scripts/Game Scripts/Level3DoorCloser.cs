using UnityEngine;
using System.Collections.Generic;

public class Level3DoorCloser : MonoBehaviour
{
    public GameObject Door;

    // Door states (using your exact coordinates)
    private Vector3 _closedPosition = new Vector3(-22.65138f, -2.0802f, -63.7955f);
    private Quaternion _closedRotation = new Quaternion(0f, -0.7071068f, 0f, 0.7071068f);
    private Vector3 _openPosition = new Vector3(-18.81138f, -1.48f, -61.4355f);
    private Quaternion _openRotation = new Quaternion(0f, -0.9659258f, 0f, 0.2588191f);

    private HashSet<string> _touchedPlayers = new HashSet<string>();
    private const int RequiredPlayerCount = 2;
    private bool _isDoorClosed = false;
    private float _closeDelay = 1f; // 3 second delay before closing

    private void OnTriggerEnter(Collider other)
    {
        if (!_isDoorClosed && other.CompareTag("Player") && !_touchedPlayers.Contains(other.name))
        {
            _touchedPlayers.Add(other.gameObject.name);

            if (_touchedPlayers.Count >= RequiredPlayerCount)
            {
                Invoke("CloseDoor", _closeDelay);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    _touchedPlayers.Remove(other.name);
        //}
    }

    public void OpenDoor()
    {
        if (Door != null)
        {
            Door.transform.position = _openPosition;
            Door.transform.rotation = _openRotation;
            _isDoorClosed = false;
            _touchedPlayers.Clear();
            CancelInvoke("CloseDoor"); // Cancel pending close
        }
    }

    private void CloseDoor()
    {
        if (Door != null && !_isDoorClosed)
        {
            Door.transform.position = _closedPosition;
            Door.transform.rotation = _closedRotation;
            _isDoorClosed = true;
        }
    }
}