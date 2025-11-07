
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float StartingHealth;
    public float CurrentHealth 
    { 
        get => _health;
        set
        {
            _health = value;
            Debug.Log(_health);
            CheckState();
        }
    }

    private float _health;
   

    [SerializeField] UnityEvent Dead;

    private void Awake()
    {
        RefillHealth();
    }

    //Call these functions using Send Message 
    private void Damage(float Amount) => CurrentHealth -= Amount;
    private void Heal(float Amount) => CurrentHealth += Amount;
    private void RefillHealth() => CurrentHealth = StartingHealth;


    private void CheckState()
    {

    }



}
