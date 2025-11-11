
using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action Damaged;
    public event Action Healed;

    [SerializeField] float StartingHealth;
    [SerializeField] float MaxHealth;
    public float CurrentHealth 
    { 
        get => _health;
        set
        {
            _health = value;
            _health = Mathf.Clamp(value, 0, MaxHealth + BonusMaxHealth);


            CheckState();
        }
    }

    private float _health;


    [SerializeField] UnityEvent Dead;

    public float BonusMaxHealth;
    public float DamageReduction;


    private void Awake()
    {
        RefillHealth();
    }

    //Call these functions using Send Message 
    private void Damage(float Amount)
    {
        CurrentHealth -= Amount - DamageReduction;
        if(Damaged != null) Damaged.Invoke();
    }
    private void Heal(float Amount) 
    {
        CurrentHealth += Amount; 
        if(Healed != null) Healed.Invoke();
    }
    private void RefillHealth() => CurrentHealth = StartingHealth;

    private void CheckState()
    {
        if(_health == 0)
        {
            Dead.Invoke();
        }
    }



}
