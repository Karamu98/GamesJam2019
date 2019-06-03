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
    private float m_fAttackRange;

    [SerializeField]
    private float m_fDamageDealtPerHit;

    [SerializeField]
    private float m_fAttackDelay;
    private float m_fCurrentAttackDelay;



    [SerializeField]
    private List<GameObject> m_lgoAttackableObjects;

    private NavMeshAgent m_nmaNavAgent;

    private Transform m_tTarget;
    private void Start()
    {
        m_nmaNavAgent = GetComponent<NavMeshAgent>();
        if (m_nmaNavAgent == null)
        {
            m_nmaNavAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        m_fCurrentHealth = m_fMaxHealth;
        m_fCurrentAttackDelay = m_fAttackDelay;
        ChooseNewTarget();
    }      

    public void ChooseNewTarget()
    {
        SetTarget(GetClosestAttackableObject());
    }

    private void GetTargets()
    {
        m_lgoAttackableObjects = new List<GameObject>();
        CS_AIAttackableObjectComponent[] csObjectArray = FindObjectsOfType<CS_AIAttackableObjectComponent>();
        foreach (CS_AIAttackableObjectComponent csObject in csObjectArray)
        {
            m_lgoAttackableObjects.Add(csObject.gameObject);
        }

    }

    private Transform GetClosestAttackableObject()
    {
        GetTargets();
        Transform tClosestTarget = transform;
        foreach (GameObject goObject in m_lgoAttackableObjects)
        {
            if(tClosestTarget == transform)
            {
                tClosestTarget = goObject.transform;
            }
            else
            {
                if(Vector3.Distance(goObject.transform.position, transform.position) <=
                    Vector3.Distance(tClosestTarget.position, transform.position))
                {
                    tClosestTarget = goObject.transform;
                }
            }
        }
        return tClosestTarget;
    }

    public bool InAttackRange()
    {
        if(Vector3.Distance(transform.position, m_tTarget.position) <= m_fAttackRange)
        {
            return true;
        }
        return false;
    }

    public void DeathSequence()
    {
        Destroy(gameObject);
    }

    public void MoveAgent()
    {
        m_nmaNavAgent.SetDestination(m_tTarget.position);
        if(InAttackRange())
        {
            m_nmaNavAgent.isStopped = true;
        }
        else
        {
            m_nmaNavAgent.isStopped = false;
        }
    }

    public bool DeathCheck()
    {
        if (m_fCurrentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public bool TargetNullCheck()
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


    public bool IsTargetAPlatform()
    {
        if(m_tTarget.GetComponent<PlayerPlatform>() == null)
        {
            return false;
        }
        return true;
    }

    public bool IsTargetAPlayer()
    {
        if (m_tTarget.GetComponent<CS_PlayerController>() == null)
        {
            return false;
        }
        return true;
    }

    public virtual void AttackPlatform()
    {
        m_tTarget.GetComponent<PlayerPlatform>().TakeDamage((int)m_fDamageDealtPerHit);
        ResetAttackDelay();
    }

    public bool AttackDelayCheck()
    {
        if(m_fCurrentAttackDelay <= 0)
        {
            return true;
        }
        return false;
    }

    public bool CanAgentAttack()
    {
        if(AttackDelayCheck() && InAttackRange())
        {
            return true;
        }
        return false;
    }

    public Transform GetTargetRef()
    {
        return m_tTarget;
    }

    public void UpdateAttackDelay()
    {
        if(m_fCurrentAttackDelay > -1)
        {
            m_fCurrentAttackDelay -= Time.deltaTime;
        }
    }
    public void ResetAttackDelay()
    {
        m_fCurrentAttackDelay = m_fAttackDelay;

    }

    public float GetDamage()
    {
        return m_fDamageDealtPerHit;
    }


}
