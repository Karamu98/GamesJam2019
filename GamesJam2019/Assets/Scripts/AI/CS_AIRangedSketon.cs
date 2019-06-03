using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AIRangedSketon : CS_AIBase
{
    [Header("Settings")]

    [Header("Prefabs")]
    [SerializeField]
    private GameObject m_goArrowPrefab;

    private void FixedUpdate()
    {
        MoveAgent();
        if (DeathCheck())
        {
            DeathSequence();
        }

        if (!TargetNullCheck())
        {
            UpdateAttackDelay();
            AttackTarget();
        }

    }


    public override void MoveAgent()
    {
        if (TargetNullCheck())
        {
            ChooseNewTarget();
            return;
        }
        else
        {
            GetToSafeDistance();
        }
        if (InAttackRange() && SafeDistanceCheck())
        {
            m_nmaNavAgent.isStopped = true;
        }
        else
        {
            m_nmaNavAgent.isStopped = false;
        }
    }

    private void GetToSafeDistance()
    {
        if(InAttackRange())
        {
            if (!SafeDistanceCheck())
            {
                transform.LookAt(GetTargetRef());
                Vector3 v3SafeTarget = -transform.forward * 10.0f;
                m_nmaNavAgent.SetDestination(v3SafeTarget);
            }
            else
            {
                m_nmaNavAgent.SetDestination(GetTargetRef().position);

            }
        }
        else
        {
            m_nmaNavAgent.SetDestination(GetTargetRef().position);

        }
    }

    private bool SafeDistanceCheck()
    {
        if(Vector3.Distance(transform.position, GetTargetRef().position) <= (m_fAttackRange / 2))
        {
            return false;
        }
        return true;
    }

    private void AttackTarget()
    {
        if (CanAgentAttack())
        {
            if (IsTargetAPlatform())
            {
                AttackPlatform();
            }
        }
    }

    public override void AttackPlatform()
    {
        transform.LookAt(GetTargetRef());
        GameObject goProjectile = Instantiate(m_goArrowPrefab);
        goProjectile.transform.position = transform.position;
        goProjectile.transform.position += transform.forward * 2.0f;
        goProjectile.GetComponent<CS_AIProjectile>().Initialise(GetTargetRef(), GetDamage());
        ResetAttackDelay();
    }
}
