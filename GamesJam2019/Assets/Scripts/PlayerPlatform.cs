using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] shipPrefabs;
    [SerializeField] private int maxHealth = 100;

    private int health;
    private GameObject mesh;
    private int currentMesh = -1;
  

    public void AddNewPart(int a_iPartHealth)
    {
        health += a_iPartHealth;
    }

    private void Awake()
    {
        health = maxHealth;
        MakeMesh(2);
    }

    private void MakeMesh(int a_selection)
    {
        Destroy(mesh);
        mesh = Instantiate(shipPrefabs[a_selection]);
        mesh.transform.position = gameObject.transform.position;
        currentMesh = a_selection;
    }

    private void Update()
    {
        float percentage = (float)health / (float)maxHealth;

        if(percentage >= 0.6f && currentMesh != 2)
        {
            MakeMesh(2);
        }
        else if(percentage >= 0.3f && percentage < 0.6f && currentMesh != 1)
        {
            MakeMesh(1);
        }
        else if(percentage < 0.3f && currentMesh != 0)
        {
            MakeMesh(0);
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
