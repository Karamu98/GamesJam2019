using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed = 90;
    [SerializeField] private float bobSpeed = 2;
    [SerializeField] private float bobDistance = 5;

    // Cache
    float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate by speed
        transform.Rotate(0, speed * Time.deltaTime, 0);

        // Move the object
        transform.position = new Vector3(transform.position.x, startY + (Mathf.Sin(Time.time * bobSpeed) * bobDistance), transform.position.z);
    }
}
