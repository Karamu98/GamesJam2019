using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_PlayerController : MonoBehaviour, IDamageable
{
    private int iPlayerNum;
    private Rigidbody nav;
    private float fSpeed;
    private bool bStunned;
    private float fTimer;

    float fMeleeAngle = 45; // 45 degrees, Melee range
    float fMeleeDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<Rigidbody>();
        fSpeed = 10f;
        bStunned = false;
        fTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(!bStunned)
        {
            PlayerMovement();
            if(Input.GetButtonDown("Melee" + iPlayerNum))
            {
                MeleeAttack();
            }
            else if(Input.GetAxis("Range" + iPlayerNum) == 1)
            {
                RangeAttack();
            }
        }
        else
        {
            PassOut();
        }
    }

    private void PlayerMovement()
    {
       //gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal" + iPlayerNum) * fSpeed,
        // 0, Input.GetAxis("Vertical" + iPlayerNum) * fSpeed);
        nav.velocity = new Vector3(Input.GetAxis("Horizontal" + iPlayerNum) * fSpeed,
         0, Input.GetAxis("Vertical" + iPlayerNum) * fSpeed);
        CalculateRotation();
    }

    private void CalculateRotation()
    {
        float fx = Input.GetAxis("RotateHorizontal" + iPlayerNum);
        Vector3 Look = new Vector3(fx, 0, -Input.GetAxisRaw("RotateVertical" + iPlayerNum));
        Look += gameObject.transform.position;
        Debug.Log(fx);
        gameObject.transform.LookAt(Look);

    }

    private void MeleeAttack()
    {
        CS_AIBase[] Enemies = FindObjectsOfType<CS_AIBase>();
        foreach(CS_AIBase ai in Enemies)
        {
            float angle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.position - ai.transform.position);
            if (angle > -fMeleeAngle && angle < fMeleeAngle)
            {
                ai.DamageAgent(fMeleeDamage);
            }
        }
    }

    private void RangeAttack()
    {

    }

    public void SetPlayerNumber(int a_iNum)
    {
        iPlayerNum = a_iNum;
    }

    private void PassOut()
    {
        fTimer -= Time.deltaTime;
        if(fTimer <= 0)
        {
            bStunned = false;
            fTimer = 3;
        }
    }


    public void TakeDamage(int a_damageToTake)
    {
        if (!bStunned)
        {
            bStunned = true;
        }
    }

    public void Heal(int a_healthToHeal)
    {
        throw new System.NotImplementedException();
    }
}
