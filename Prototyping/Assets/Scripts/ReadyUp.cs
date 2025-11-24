using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ReadyUp : MonoBehaviour
{
    [SerializeField] GameObject GoldCount;
    [SerializeField] GameObject NextBoss;
    [SerializeField] GameObject[] Bosses;

   
    [SerializeField] Camera Cam;
    [SerializeField] TextMeshProUGUI Label;

    
    [SerializeField] Transform Destination;

    [SerializeField] GameObject ThroneRoom,ChestRoom,LavaPit;
    

    [SerializeField] Transform shopCam;
    [SerializeField] Transform shopSpawn;

    [SerializeField] ShopSpawner ShopSpawner;

    [SerializeField] public UnityEvent BossSpawn;

    [SerializeField] List<GameObject> Players = new List<GameObject>();

    public string targetTag = "Player";
    private bool db = false;
    private HashSet<GameObject> touchingObjects = new HashSet<GameObject>();
    private GameObject[] taggedObjects;
    private GameObject _activeBoss;
    private GameManager Manager;

    private void Start()
    {
        taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        Manager = GameManager.instance;
    }
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
        if(_activeBoss == null)
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
        Manager.CountPlayers();
        Collider2D collider = null;
        GoldCount.SetActive(false);
        switch (NextBoss.GetComponent<BossBase>().Arena)
        {
            case BossBase.Arenas.ChestRoom:
                Destination.position = ChestRoom.transform.position;
                collider = ChestRoom.GetComponent<Collider2D>();
                break;         
            case BossBase.Arenas.ThroneRoom:
                Destination.position = ThroneRoom.transform.position;
                collider = ThroneRoom.GetComponent<Collider2D>();
                break;
            case BossBase.Arenas.LavaPit:
                Destination.position = LavaPit.transform.position;
                collider = LavaPit.GetComponent<Collider2D>();
                break;
        }

        _activeBoss = Instantiate(NextBoss, Destination.transform.position, Quaternion.identity);

        _activeBoss.GetComponent<Health>().DeadEvent += BossDied;

        _activeBoss.GetComponent<BossBase>().SetArena(collider);

        Cam.transform.position = new Vector3(Destination.position.x, Destination.position.y, -10);
        Cam.orthographicSize = 8.0f;
        foreach (GameObject obj in taggedObjects)
        {
            obj.transform.position = Destination.position;
        }
    }

    private void BossDied()
    {
        _activeBoss.GetComponent<Health>().DeadEvent -= BossDied;

        Manager.BossesKilled += 1;

        Manager.ChangeGold(100 * (Manager.BossesKilled + 1));

        Destroy(_activeBoss);
        ToShop();
    }

    public void ToShop()
    {
       
        ShopSpawner.NewShop();
        GoldCount.SetActive(true);
        NextBoss = null;
        SelectBoss();
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
            obj.GetComponent<Health>().RefillHealth();
        }
        db = false;
    }
}
