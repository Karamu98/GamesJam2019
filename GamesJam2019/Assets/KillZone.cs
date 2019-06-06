using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.transform.parent.GetComponentInChildren<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(999999999, gameObject);
        }
    }
}
