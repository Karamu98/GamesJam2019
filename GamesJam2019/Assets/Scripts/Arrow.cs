using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = 2.0f;

    // Cache
    private int attackDamage;
    private Rigidbody rBody;
    private GameObject shooter;

    // Start is called before the first frame update
    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rBody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Arrow>() != null)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            return;
        }

        rBody.useGravity = true;

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamage(attackDamage, shooter);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Arrow>() != null)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
            return;
        }
        //rBody.isKinematic = true;
        //Vector3 scale = rBody.gameObject.transform.localScale;
        //rBody.gameObject.transform.parent = other.transform;
        //rBody.gameObject.transform.localScale = scale;
        rBody.constraints = RigidbodyConstraints.FreezeAll;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage, shooter);
        }
    }

    public void Initialise(int a_damage, GameObject a_shooter)
    {
        shooter = a_shooter;
        attackDamage = a_damage;

        Destroy(gameObject, lifeTime);
    }
}