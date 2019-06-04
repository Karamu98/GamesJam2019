using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AIProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float m_fSpeed;

    [SerializeField]
    private Transform m_tTarget;

    [SerializeField]
    private float m_fDamage;

    [SerializeField]
    private float m_fLifeTime = 2.0f;

    private Rigidbody m_rbRigidBodyRef;

    // Start is called before the first frame update
    private void Start()
    {
        m_rbRigidBodyRef = GetComponent<Rigidbody>();
        if (m_rbRigidBodyRef == null)
        {
            m_rbRigidBodyRef = gameObject.AddComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        m_rbRigidBodyRef.velocity = transform.forward * m_fSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CS_AIAttackableObjectComponent>() != null)
        {
            if (collision.gameObject.GetComponent<PlayerPlatform>() != null)
            {
                collision.gameObject.GetComponent<PlayerPlatform>().TakeDamage((int)m_fDamage);
            }
        }
        Destroy(gameObject);
    }

    public void Initialise(Transform a_tTarget, float a_fDamage)
    {
        m_tTarget = a_tTarget;
        transform.LookAt(m_tTarget);
        m_fDamage = a_fDamage;

        m_fSpeed = Vector3.Distance(a_tTarget.position, transform.position) * 2.0f;

        Destroy(gameObject, m_fLifeTime);
    }
}