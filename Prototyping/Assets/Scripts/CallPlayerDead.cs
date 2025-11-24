using UnityEngine;

public class CallPlayerDead : MonoBehaviour
{
   public void Dead()
    {
        GameManager.instance.PlayerDied(gameObject);
    }
}
