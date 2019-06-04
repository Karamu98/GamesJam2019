using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int a_damageToTake, GameObject a_instigator);
    void Heal(int a_healthToHeal, GameObject a_instigator);
}
