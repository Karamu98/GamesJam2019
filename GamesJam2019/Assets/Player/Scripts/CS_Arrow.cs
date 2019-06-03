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
    private float m_fDamage;

    private Rigidbody m_rbRigidBodyRef;

    private GameObject shooter;
    Vector3 v3Look;

    private float fLifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        m_rbRigidBodyRef = GetComponent<Rigidbody>();
        if (m_rbRigidBodyRef == null)
        {
            m_rbRigidBodyRef = gameObject.AddComponent<Rigidbody>();
        }
        fLifeTimer = 3;
    }

    private void FixedUpdate()
    {
        m_rbRigidBodyRef.velocity = v3Look * m_fSpeed;
        fLifeTimer -= Time.deltaTime;
        if(fLifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CS_AIBase>() != null)
        {
            collision.gameObject.GetComponent<CS_AIBase>().DamageAgent((int)m_fDamage, shooter);
            Destroy(gameObject);
        }
    }

    public void Initialise(Transform a_tTarget, float a_fDamage, GameObject a_gPlayer)
    {
        m_tTarget = a_tTarget;
        shooter = a_gPlayer;
        v3Look = -m_tTarget.forward * 3;
        //transform.LookAt(v3Look);
        transform.rotation = m_tTarget.rotation;
        GetComponent<Rigidbody>().freezeRotation = true;
        m_fDamage = a_fDamage;
    }
}
