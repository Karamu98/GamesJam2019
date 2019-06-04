using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float a_fDamageToTake, GameObject a_goInstigator);
    void Heal(float a_fHealthToHeal, GameObject a_goInstigator);
}
