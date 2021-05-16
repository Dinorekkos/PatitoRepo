using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntity : MonoBehaviour
{
    public void MakeDamage(int damageAmount)
    {
        //Remove some health
        Health -= damageAmount;

        //If healt is 0 or less than 0
        if (Health <= 0)
        {

        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public int Health
    {
        get;
        private set;
    }

    public int MaxHealth = 4;

    private void Start()
    {
        Health = MaxHealth;
    }
}
