using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour, IDamageable
{
    private int health;
    private int maxHealth;

    public void Heal(int a_healthToHeal)
    {

        throw new System.NotImplementedException();
    }

    public void TakeDamage(int a_damageToTake)
    {
        //throw new System.NotImplementedException();

        health -= a_damageToTake;

        // Death
        if(health <= 0)
        {
            SessionManager.OnPlayerLose();
        }
    }

    public void AddNewPart(int a_partHealth)
    {
        maxHealth += a_partHealth;
    }

    private void Awake()
    {
        
    }
}
