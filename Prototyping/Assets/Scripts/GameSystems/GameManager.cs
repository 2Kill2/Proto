using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int _gold;
    private float _bossesKilled = 0;
  

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

    public void CheckPlayersAlive()
    {
        //Add logic to check the number of players alive, called 
    }


}
