using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ReadyUp : MonoBehaviour
{
    [SerializeField] GameObject SlimeKing;
    [SerializeField] GameObject Cam;
    [SerializeField] TextMeshProUGUI Label;

    //Slime, script uses this as default
    [SerializeField] Transform Destination;
    [SerializeField] Transform DestinationCam;
    [SerializeField] GameObject DestinationBoss;

    //Paladin
    [SerializeField] Transform Destination2;
    [SerializeField] Transform DestinationCam2;
    [SerializeField] GameObject DestinationBoss2;

    //Lich
    [SerializeField] Transform Destination3;
    [SerializeField] Transform DestinationCam3;
    [SerializeField] GameObject DestinationBoss3;

    //Dragon
    [SerializeField] Transform Destination4;
    [SerializeField] Transform DestinationCam4;
    [SerializeField] GameObject DestinationBoss4;

    [SerializeField] Transform shopCam;
    [SerializeField] Transform shopSpawn;

    [SerializeField] public UnityEvent BossSpawn;

    [SerializeField] List<GameObject> Players = new List<GameObject>();

    public string targetTag = "Player";
    private bool db = false;
    private HashSet<GameObject> touchingObjects = new HashSet<GameObject>();
    private GameObject[] taggedObjects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        touchingObjects.Add(collision.gameObject);
        Debug.Log(touchingObjects.Count);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchingObjects.Remove(collision.gameObject);

    }

    // You can call this method to get the current count of touching objects
    public int GetTouchingObjectCount()
    {
        return touchingObjects.Count;
    }

    private void Update()
    {
        taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        Label.text = touchingObjects.Count+"/"+taggedObjects.Length;
        if (touchingObjects.Count > 0 && touchingObjects.Count == taggedObjects.Length && db==false)
        {
            db = true;
            MoveUp();
        }
    }

    private void CountPlayers()
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        var objects = FindObjectsByType<GameObject>(
            FindObjectsSortMode.None
        );

        foreach (var obj in objects)
        {
            if (obj.layer == playerLayer)
            {
                Players.Add(obj);
            }
        }
    }

    private void MoveUp()
    {
        
        if (DestinationBoss != null)
        {
            DestinationBoss.gameObject.SetActive(true);
        }
        Cam.transform.position = DestinationCam.position;
        Cam.GetComponent<Camera>().orthographicSize = 8.0f;
        foreach (GameObject obj in taggedObjects)
        {
            obj.transform.position = Destination.position;
        }
    }

    public void MoveDown()
    {
        StartCoroutine(WaitAndMove());
    }
    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(5.0f);
        Cam.transform.position = shopCam.position;
        Cam.GetComponent<Camera>().orthographicSize = 5.0f;
        CountPlayers();
        foreach (GameObject obj in Players)
        {
            obj.transform.position = shopSpawn.position;
        }
        db = false;
    }

    public void SlimeDefeated()
    {
        DestinationCam = DestinationCam2;
        DestinationBoss = DestinationBoss2;
        Destination = Destination2;
    }

    public void PaladinDefeated()
    {
        DestinationCam = DestinationCam3;
        DestinationBoss = DestinationBoss3;
        Destination = Destination3;
    }
    public void LichDefeated()
    {
        DestinationCam = DestinationCam4;
        DestinationBoss = DestinationBoss4;
        Destination = Destination4;
    }
}
