using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour, IDamageable
{
    private int health;
    private int maxHealth;


    public void AddNewPart(int a_partHealth)
    {
        maxHealth += a_partHealth;
    }

    private void Awake()
    {
        
    }

    public void TakeDamage(int a_damageToTake, GameObject a_instigator)
    {
        health -= a_damageToTake;

        // Death
        if (health <= 0)
        {
            SessionManager.OnPlayerLose();
        }
    }

    public void Heal(int a_healthToHeal, GameObject a_instigator)
    {
        throw new System.NotImplementedException();
    }
}
