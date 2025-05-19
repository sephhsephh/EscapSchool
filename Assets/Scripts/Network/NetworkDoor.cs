using Unity.Netcode;
using UnityEngine;

public class NetworkDoor : NetworkBehaviour
{
    private NetworkVariable<bool> isDoorActive = new NetworkVariable<bool>(true);

    public override void OnNetworkSpawn()
    {
        isDoorActive.OnValueChanged += OnDoorStateChanged;
        UpdateDoorState(isDoorActive.Value);
    }

    private void OnDoorStateChanged(bool oldValue, bool newValue)
    {
        UpdateDoorState(newValue);
    }

    private void UpdateDoorState(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetDoorState(bool state)
    {
        if (IsServer)
        {
            isDoorActive.Value = state;
        }
    }
}