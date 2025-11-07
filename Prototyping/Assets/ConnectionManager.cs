using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] public GameObject playerPrefab;
    private InputUser inputUser;

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added)
        {
            Debug.Log($"New device connected: {device.displayName}");
            if (device is Gamepad)
            {
                InstantiatePlayer(device);
            }
        }
        else if (change == InputDeviceChange.Removed)
        {
            Debug.Log($"Device disconnected: {device.displayName}");
        }
    }

    private void InstantiatePlayer(InputDevice device)
    {
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log($"Instantiated player for device: {device.displayName}");
    }
}
