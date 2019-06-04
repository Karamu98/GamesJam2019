using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour, IDamageable
{
    [SerializeField] private Mesh[] shipMeshes;
    [SerializeField] private int maxHealth = 100;

    private int health;
    private MeshFilter meshFilter;
  

    public void AddNewPart(int a_iPartHealth)
    {
        health += a_iPartHealth;
    }

    private void Awake()
    {
        health = maxHealth;
        meshFilter = GetComponent<MeshFilter>();


    }

    private void Update()
    {
        float percentage = health / maxHealth;

        if(percentage >= 0.7f)
        {
            meshFilter.mesh = shipMeshes[2];
        }
        else if(percentage >= 0.5f && percentage < 0.7f)
        {
            meshFilter.mesh = shipMeshes[1];
        }
        else
        {
            meshFilter.mesh = shipMeshes[0];
        }
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
