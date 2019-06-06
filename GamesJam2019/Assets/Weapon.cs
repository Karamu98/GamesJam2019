using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject wielder;
    private bool isSwinging = false;
    private int damage;


    public void Initialise(GameObject a_owner, int a_damage)
    {
        wielder = a_owner;
        damage = a_damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == wielder)
        {
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage, wielder);
        }
    }

}
