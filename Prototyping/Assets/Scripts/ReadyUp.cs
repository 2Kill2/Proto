using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ReadyUp : MonoBehaviour
{
    [SerializeField] GameObject GoldCount;
    [SerializeField] GameObject NextBoss;
    [SerializeField] GameObject[] Bosses;

    [SerializeField] Camera Cam;
    [SerializeField] TextMeshProUGUI Label;

    [SerializeField] Transform Destination;

    [SerializeField] GameObject ThroneRoom, ChestRoom, LavaPit;

    [SerializeField] Transform shopCam;
    [SerializeField] GameObject shopSpawn;

    [SerializeField] ShopSpawner ShopSpawner;

    [SerializeField] public UnityEvent BossSpawn;

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchingObjects.Remove(collision.gameObject);
    }

    private void SelectBoss()
    {
        if (NextBoss == null)
            NextBoss = Bosses[Random.Range(0, Bosses.Length)];
    }

    private void Update()
    {
        if (_activeBoss == null)
        {
            taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        }

        Label.text = touchingObjects.Count + "/" + taggedObjects.Length;

        if (touchingObjects.Count > 0 &&
            touchingObjects.Count == taggedObjects.Length &&
            db == false)
        {
            db = true;
            ToArena();
        }
    }

    private void ToArena()
    {
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

        var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Player"))
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

      
        var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Player"))
            {
                obj.transform.position = new Vector3(0, 0, 0);
                obj.GetComponent<Health>().RefillHealth();
            }
        }

        db = false;
    }
}
