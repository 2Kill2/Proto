using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int _gold;
    private float _bossesKilled = 0;

    [SerializeField] List<GameObject> Players = new List<GameObject>();
    [SerializeField] UnityEvent AllPlayersDead;
    public float BossesKilled
    {
        get { return _bossesKilled; }
        set { _bossesKilled = value; }
    }
    public int Gold => _gold;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ChangeGold(int value) => _gold += value;


    /// <summary>
    /// Call me when the battle starts
    /// </summary>
    /// <returns></returns>
    public void CountPlayers()
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

    /// <summary>
    /// Called when a player is killed
    /// </summary>
    public void PlayerDied(GameObject player)
    {
        if(player.layer == LayerMask.NameToLayer("Player"))
        {
            Players.Remove(player);
        }

        if (Players.Count <= 0)
        {


        }
    }
    public void SetPlayersActive()
    {
        foreach (GameObject player in Players)
        {
            player.SetActive(true);

        }
    }
}
