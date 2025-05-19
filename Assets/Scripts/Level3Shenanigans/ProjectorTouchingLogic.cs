using Unity.Netcode;
using UnityEngine;

public class ProjectorTouchingLogic : NetworkBehaviour
{
    public NetworkVariable<bool> isBeingTouched = new NetworkVariable<bool>();

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            isBeingTouched.Value = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            isBeingTouched.Value = false;
        }
    }
}