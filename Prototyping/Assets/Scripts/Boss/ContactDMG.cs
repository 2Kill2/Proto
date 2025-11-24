using UnityEngine;

public class ContactDMG : MonoBehaviour
{
    [SerializeField] float Damage;

    private void Start()
    {
        Damage *= (GameManager.instance.BossesKilled + 1)* 0.5f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        collision.gameObject.SendMessage("Damage", Damage,SendMessageOptions.DontRequireReceiver);
    }
}
