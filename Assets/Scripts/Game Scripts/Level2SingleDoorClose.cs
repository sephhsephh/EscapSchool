using UnityEngine;
using System.Collections.Generic;

public class Level2SingleDoorClose : MonoBehaviour
{
    public GameObject Door;

    // Door states (using your exact coordinates)
    private Vector3 _closedPosition = new Vector3(-0.11f, 5.024329f, 0.11f);
    private Quaternion _closedRotation = new Quaternion(-89.98f, 0f, 0f, 0.7071068f);
    private Vector3 _openPosition = new Vector3(2.66f, 5.024329f, 3.3f);
    private Quaternion _openRotation = new Quaternion(0f, -90f, 0f, 0f);

    
    private bool _isDoorClosed = false;
    private float _closeDelay = 1f; // 3 second delay before closing

    private void OnTriggerEnter(Collider other)
    {
        if (!_isDoorClosed)
        {
            Invoke("CloseDoor", _closeDelay);
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