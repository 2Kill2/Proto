using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int _gold;

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
