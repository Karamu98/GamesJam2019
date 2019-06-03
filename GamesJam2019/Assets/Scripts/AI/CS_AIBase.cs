using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_AIBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float m_fMaxHealth = 100;

    private float m_fCurrentHealth;

    [SerializeField]
    private float m_fSpeed;

    [SerializeField]
    private List<CS_AIAttackableObjectComponent> m_csAttackableObjects;

    private NavMeshAgent m_nmaNavAgent;

    private Transform m_tTarget;
    private void Start()
    {
        
    }

    public void InitialiseAgent()
    {
        m_nmaNavAgent = GetComponent<NavMeshAgent>();
        if (m_nmaNavAgent == null)
        {
            m_nmaNavAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        m_fCurrentHealth = m_fMaxHealth;
    }


    private void FixedUpdate()
    {
        MoveAgent();
        if(DeathCheck())
        {
            DeathSequence();
        }
    }

    public void ChooseNewTarget()
    {
        SetTarget(GetClosestAttackableObject());
    }

    private void GetTargets()
    {
        m_csAttackableObjects = new List<CS_AIAttackableObjectComponent>();
        CS_AIAttackableObjectComponent[] csObjectArray = FindObjectsOfType<CS_AIAttackableObjectComponent>();
        foreach (CS_AIAttackableObjectComponent csObject in csObjectArray)
        {
            m_csAttackableObjects.Add(csObject);
        }
    }

    private Transform GetClosestAttackableObject()
    {
        GetTargets();
        Transform tClosestTarget = transform;
        foreach (CS_AIAttackableObjectComponent csObject in m_csAttackableObjects)
        {
            if(tClosestTarget == transform)
            {
                tClosestTarget = csObject.transform;
            }
            else
            {
                if(Vector3.Distance(csObject.transform.position, transform.position) <=
                    Vector3.Distance(tClosestTarget.position, transform.position))
                {
                    tClosestTarget = csObject.transform;
                }
            }
        }
        return tClosestTarget;
    }

    private void DeathSequence()
    {
        Destroy(gameObject);
    }

    private void MoveAgent()
    {
        m_nmaNavAgent.SetDestination(m_tTarget.position);
    }

    private bool DeathCheck()
    {
        if (m_fCurrentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    private bool TargetNullCheck()
    {
        if(m_tTarget == null)
        {
            return true;
        }
        return false;
    }

    public void SetTarget(Transform a_tTarget)
    {
        m_tTarget = a_tTarget;
    }

    public void KillAgent()
    {
        m_fCurrentHealth = -1.0f;
    }

    public void DamageAgent(float a_fDamage)
    {
        m_fCurrentHealth -= a_fDamage;
        
    }


}
