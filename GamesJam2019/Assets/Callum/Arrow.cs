using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20;
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
        Vector3 force = transform.forward * speed;
        rBody.velocity = new Vector3(force.x, rBody.velocity.y, force.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Arrow>() != null || other.gameObject.GetComponent<Collider>().isTrigger == true)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
            return;
        }
        
        rBody.isKinematic = true;
        rBody.gameObject.transform.parent = other.transform;
        
        IDamageable damageable = other.gameObject.transform.root.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage, shooter);
            return;
        }

        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        if (otherBody != null)
        {
            Vector3 force = gameObject.transform.forward * rBody.mass;
            otherBody.AddForceAtPosition(force, gameObject.transform.position, ForceMode.Impulse);
        }

        GetComponent<Collider>().enabled = false;

    }

    public void Initialise(int a_damage, GameObject a_shooter)
    {
        shooter = a_shooter;
        attackDamage = a_damage;

        Destroy(gameObject, lifeTime);
    }
}