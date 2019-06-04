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
    public float m_fAttackRange;

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private float m_fAttackDelay;
    private float m_fCurrentAttackDelay;

    private CS_AIAnimationHandler m_ahAnimationHandler;

    private List<GameObject> m_lgoAttackableObjects;

    [HideInInspector]
    public NavMeshAgent m_nmaNavAgent;

    private Transform m_tTarget;
    private void Start()
    {
        m_nmaNavAgent = GetComponent<NavMeshAgent>();
        if (m_nmaNavAgent == null)
        {
            m_nmaNavAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        m_ahAnimationHandler = GetComponent<CS_AIAnimationHandler>();
        if (m_ahAnimationHandler == null)
        {
            m_ahAnimationHandler = gameObject.AddComponent<CS_AIAnimationHandler>();
        }
        m_fCurrentHealth = m_fMaxHealth;
        m_fCurrentAttackDelay = m_fAttackDelay;
        ChooseNewTarget();
    }      

    public void ChooseNewTarget()
    {
        int iRandom = Random.Range(0, 2);
        if(iRandom == 0)
        {
            PlayerPlatform tPlatform = FindObjectOfType<PlayerPlatform>();
            if(tPlatform != null)
            {
                SetTarget(tPlatform.transform);
            }
            else
            {
                ChooseNewTarget();
            }
        }
        else
        {
            Transform tPlayer = GetClosestPlayerObject();
            if (tPlayer != null && tPlayer != transform)
            {
                SetTarget(tPlayer);
            }
            else
            {
                ChooseNewTarget();
            }
        }
        //SetTarget(GetClosestAttackableObject());
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

    private Transform GetClosestPlayerObject()
    {
        CS_PlayerController[] m_lcsPlayers = FindObjectsOfType<CS_PlayerController>();
        Transform tClosestTarget = transform;
        foreach (CS_PlayerController csObject in m_lcsPlayers)
        {
            if (tClosestTarget == transform)
            {
                tClosestTarget = csObject.transform;
            }
            else
            {
                if (Vector3.Distance(csObject.transform.position, transform.position) <=
                    Vector3.Distance(tClosestTarget.position, transform.position) &&
                    csObject.bInvunerable == false)
                {
                    tClosestTarget = csObject.transform;
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

    public virtual void MoveAgent()
    {
        if(TargetNullCheck() || (IsTargetAPlayer() && GetTargetRef().GetComponent<CS_PlayerController>().bInvunerable))
        {
            ChooseNewTarget();
            return;
        }
        else
        {
            m_nmaNavAgent.SetDestination(m_tTarget.position);

        }
        if (InAttackRange())
        {
            m_nmaNavAgent.isStopped = true;
            AnimationSetStopped();

        }
        else
        {
            m_nmaNavAgent.isStopped = false;
            AnimationSetMoving();

        }
    }

    public void AnimationSetMoving()
    {
        m_ahAnimationHandler.Run();
    }
    public void AnimationSetStopped()
    {
        m_ahAnimationHandler.Still();
    }
    public void AnimationSetAttack()
    {
        m_ahAnimationHandler.Attack();
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

    public void DamageAgent(float a_fDamage, GameObject m_goAttacker)
    {
        m_fCurrentHealth -= a_fDamage;
        SetTarget(m_goAttacker.transform);        
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
        m_tTarget.GetComponent<PlayerPlatform>().TakeDamage(attackDamage, gameObject);
        ResetAttackDelay();
        AnimationSetAttack();
    }
    public virtual void AttackPlayer()
    {
        if(m_tTarget.GetComponent<CS_PlayerController>().bInvunerable == true)
        {
            ChooseNewTarget();
            ResetAttackDelay();
            return;
        }

        m_tTarget.GetComponent<CS_PlayerController>().TakeDamage(attackDamage, gameObject);
        ResetAttackDelay();
        AnimationSetAttack();
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
        if(AttackDelayCheck() && InAttackRange() && !TargetNullCheck())
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

    public int GetDamage()
    {
        return attackDamage;
    }


}
