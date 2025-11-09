
using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action Damaged;
    public event Action Healed;

    [SerializeField] float StartingHealth;
    public float CurrentHealth 
    { 
        get => _health;
        set
        {
            _health = value;
           
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
    private void Damage(float Amount)
    {
        CurrentHealth -= Amount;
        //Damaged.Invoke();
    }
    private void Heal(float Amount) 
    {
        CurrentHealth += Amount; 
        //Healed.Invoke();
    }
    private void RefillHealth() => CurrentHealth = StartingHealth;

    private void CheckState()
    {
        if(_health < 0)
        {
            Dead.Invoke();
        }
    }



}
