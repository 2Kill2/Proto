using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ReadyUp : MonoBehaviour
{

    [SerializeField] GameObject NextBoss;
    [SerializeField] GameObject[] Bosses;

   
    [SerializeField] Camera Cam;
    [SerializeField] TextMeshProUGUI Label;

    
    [SerializeField] Transform Destination;

    [SerializeField] GameObject ThroneRoom,ChestRoom,LavaPit;
    

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

    private void SelectBoss()
    {
        if(NextBoss == null)
            NextBoss = Bosses[Random.Range(0, Bosses.Length)];
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
            ToArena();
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

    private void ToArena()
    {
        switch (NextBoss.GetComponent<BossBase>().Arena)
        {
            case BossBase.Arenas.ChestRoom:
                Destination.position = ChestRoom.transform.position;
                break;         
            case BossBase.Arenas.ThroneRoom:
                Destination.position = ThroneRoom.transform.position;
                break;
            case BossBase.Arenas.LavaPit:
                Destination.position = LavaPit.transform.position;
                break;
        }
        
        Cam.orthographicSize = 8.0f;
        foreach (GameObject obj in taggedObjects)
        {
            obj.transform.position = Destination.position;
        }
    }

    public void ToShop()
    {
        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(5.0f);
        Cam.transform.position = shopCam.position;
        Cam.orthographicSize = 5.0f;
        CountPlayers();
        foreach (GameObject obj in Players)
        {
            obj.transform.position = shopSpawn.position;
        }
        db = false;
    }
}
