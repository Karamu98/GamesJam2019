using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private float activeTime;

    private float timer;
    private bool isSinking = false;

    private void Awake()
    {
        timer = activeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && isSinking == false)
        {
            isSinking = true;
            StartCoroutine(Sink());
        }
    }

    private IEnumerator Sink()
    {
        GetComponentInChildren<Collider>().enabled = false;
        yield return new WaitForSeconds(7);

        Destroy(gameObject);
    }
}
