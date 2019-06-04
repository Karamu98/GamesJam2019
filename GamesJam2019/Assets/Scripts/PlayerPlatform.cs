using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour, IDamageable
{

    private float m_fHealth;
    [SerializeField]
    private float m_fMaxHealth = 100.0f;


    public void AddNewPart(int a_iPartHealth)
    {
        m_fHealth += a_iPartHealth;
    }

    private void Awake()
    {
        m_fHealth = m_fMaxHealth;
    }

    public void TakeDamage(float a_fDamageToTake, GameObject a_goInstigator)
    {
        m_fHealth -= a_fDamageToTake;

        // Death
        if (m_fHealth <= 0)
        {
            SessionManager.OnPlayerLose();
        }
    }

    public void Heal(float a_fHealthToHeal, GameObject a_goInstigator)
    {
        throw new System.NotImplementedException();
    }
}
