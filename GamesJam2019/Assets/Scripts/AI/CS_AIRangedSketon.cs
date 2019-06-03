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

        UpdateAttackDelay();
        AttackTarget();

        if(TargetNullCheck())
        {
            ChooseNewTarget();
        }
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
