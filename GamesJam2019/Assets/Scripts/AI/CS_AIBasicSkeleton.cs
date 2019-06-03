using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AIBasicSkeleton : CS_AIBase
{
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


    private void AttackTarget()
    {
        if(CanAgentAttack())
        {
            if(IsTargetAPlatform())
            {
                AttackPlatform();
            }
            else if (IsTargetAPlayer())
            {
                AttackPlayer();
            }
        }
    }


}
