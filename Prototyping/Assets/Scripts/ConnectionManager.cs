using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public Sprite p1Sprite;
    [SerializeField] public Sprite p2Sprite;
    [SerializeField] public Sprite p3Sprite;
    [SerializeField] public Sprite p4Sprite;
    [SerializeField] public GameObject p1Hud;
    [SerializeField] public GameObject p2Hud;
    [SerializeField] public GameObject p3Hud;
    [SerializeField] public GameObject p4Hud; //idc
    public int PlayerCount = 0;
    private InputUser inputUser;


    public void OnPlayerJoin()
    {
        PlayerCount++;
        GameObject.Find("PlayerObject(Clone)").name = "Player"+ PlayerCount;
        switch (PlayerCount)
        {
            case 1:
                GameObject.Find("Player1").transform.Find("Ps").GetComponent<SpriteRenderer>().sprite = p1Sprite;
                p1Hud.SetActive(true);
            break;
            case 2:
                GameObject.Find("Player2").transform.Find("Ps").GetComponent<SpriteRenderer>().sprite = p2Sprite;
                p2Hud.SetActive(true);
            break;
            case 3:
                GameObject.Find("Player3").transform.Find("Ps").GetComponent<SpriteRenderer>().sprite = p3Sprite;
                p3Hud.SetActive(true);
            break;
            case 4:
                GameObject.Find("Player4").transform.Find("Ps").GetComponent<SpriteRenderer>().sprite = p4Sprite;
                p4Hud.SetActive(true);
            break;
        }
    }

    //BELOW THIS IS OLD CODE
    void Awake()
    {
        //InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDestroy()
    {
        //InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added)
        {
            Debug.Log($"New device connected: {device.displayName}");
            if (device is Gamepad)
            {
                //InstantiatePlayer(device);
            }
        }
        else if (change == InputDeviceChange.Removed)
        {
            Debug.Log($"Device disconnected: {device.displayName}");
        }
    }

    private void InstantiatePlayer() //(InputDevice device)
    {
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //Debug.Log($"Instantiated player for device: {device.displayName}");
    }
}
