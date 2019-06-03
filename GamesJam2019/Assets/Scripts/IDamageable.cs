using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int a_damageToTake);
    void Heal(int a_healthToHeal);
}
