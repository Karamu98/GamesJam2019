using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameObject waterSurface;
    [SerializeField] private float waterDensity;
    List<Rigidbody> rigidbodies = new List<Rigidbody>();


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rBody = other.gameObject.GetComponent<Rigidbody>();

        if(rBody.tag == "Player")
        {
            return;
        }

        if (rBody != null)
        {
            rigidbodies.Add(rBody);
            rBody.drag = 10f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rBody = other.gameObject.GetComponent<Rigidbody>();

        if (rBody != null && rigidbodies.Contains(rBody))
        {
            rigidbodies.Remove(rBody);
        }
    }

    private void FixedUpdate()
    {
        float yTest = waterSurface.gameObject.transform.position.y;

        rigidbodies.RemoveAll(item => item == null || item.gameObject.GetComponent<Collider>().enabled == false);

        if(rigidbodies.Count > 0)
        {
            foreach (Rigidbody body in rigidbodies)
            {
                float bodyPosition = body.transform.position.y;

                Vector3 force = transform.up * waterDensity;
                body.AddRelativeForce(force * (yTest - bodyPosition), ForceMode.Acceleration);
            }
        }

    }
}
