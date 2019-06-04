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

    private Rigidbody m_rbRigidBodyRef;

    private GameObject shooter;
    Vector3 v3Look;

    private bool hasHit = false;

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
        fLifeTimer -= Time.deltaTime;
        if(fLifeTimer <= 0)
        {
            Destroy(gameObject);
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
        m_tTarget = a_tTarget;
        shooter = a_gPlayer;
        v3Look = -m_tTarget.forward * 3;
        transform.rotation = m_tTarget.rotation;
        GetComponent<Rigidbody>().freezeRotation = true;
        arrowDamage = a_damage;
    }
}
