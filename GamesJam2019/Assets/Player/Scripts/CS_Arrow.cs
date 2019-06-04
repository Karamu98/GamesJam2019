using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Arrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float m_fSpeed;

    [SerializeField]
    private Transform m_tTarget;

    [SerializeField]
    private int arrowDamage;

    [SerializeField]
    private float m_fFieldOfView = 90.0f;

    [SerializeField]
    private LayerMask m_lmEnemyMask;

    private Rigidbody m_rbRigidBodyRef;

    private GameObject shooter;
    Vector3 v3Look;

    private bool hasHit = false;

    private bool m_bTarget = false;

    private float fLifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        m_rbRigidBodyRef = GetComponent<Rigidbody>();
        if (m_rbRigidBodyRef == null)
        {
            m_rbRigidBodyRef = gameObject.AddComponent<Rigidbody>();
        }

        m_rbRigidBodyRef.velocity = v3Look * m_fSpeed;

        fLifeTimer = 3;
    }

    private void Update()
    {        
        if(!m_bTarget)
        {
            m_rbRigidBodyRef.velocity = v3Look * m_fSpeed;

        }
        else
        {
            m_rbRigidBodyRef.velocity = transform.forward * m_fSpeed;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasHit && collision.gameObject.GetComponent<CS_Arrow>() == null)
        {
            CS_AIBase damageable = collision.gameObject.GetComponent<CS_AIBase>();
            if (damageable != null)
            {
                damageable.TakeDamage(arrowDamage, shooter);
                Destroy(gameObject);
                return;
            }

            m_rbRigidBodyRef.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If we hit something damageable
        CS_AIBase damageable = other.gameObject.GetComponent<CS_AIBase>();
        if (damageable != null)
        {
            // Damage and destroy
            damageable.TakeDamage(arrowDamage, shooter);
            Destroy(gameObject);
            return;
        }

        // If we hit another arrow
        if(other.GetComponent<CS_Arrow>() != null)
        {
            // Do this later
            return;
        }
        hasHit = true;
        m_rbRigidBodyRef.constraints = RigidbodyConstraints.FreezeAll;

        Collider[] colliders;
        colliders = gameObject.GetComponents<Collider>();

        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
        
    }

    public void Initialise(Transform a_tTarget, int a_damage, GameObject a_gPlayer)
    {

        Debug.Log("Shoot arrrow");
        m_tTarget = a_tTarget;
        transform.forward = a_gPlayer.transform.forward;

        Transform tNewTarget = GetTarget(a_gPlayer);
        if(tNewTarget == null)
        {
            Debug.Log("null");

            v3Look = -m_tTarget.forward * 3;
            transform.rotation = m_tTarget.rotation;
        }
        else
        {
            m_tTarget = tNewTarget;
            transform.LookAt(tNewTarget);
            m_bTarget = true;

            m_fSpeed = Vector3.Distance(m_tTarget.position, transform.position) * 5.0f;

        }


        shooter = a_gPlayer;
        
        GetComponent<Rigidbody>().freezeRotation = true;
        arrowDamage = a_damage;

        //Destroy(gameObject, fLifeTimer);

    }



    private Transform GetTarget(GameObject a_goPlayer)
    {
        CS_AIBase[] csEnemyList = FindObjectsOfType<CS_AIBase>();
        foreach (CS_AIBase csEnemy in csEnemyList)
        {
            Vector3 v3DirToTarget = -(a_goPlayer.transform.position - csEnemy.transform.position ).normalized;
            if(Vector3.Angle(a_goPlayer.transform.position, csEnemy.transform.position) < m_fFieldOfView * 0.5f)
            {
                //if(Physics.Raycast(transform.position, v3DirToTarget, 1000.0f, m_lmEnemyMask))
                //{
                    Debug.Log("Found Enemy");

                    return csEnemy.transform;
                //}
            }
        }





        return null;
    }
}
