using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private int healthToAdd;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private AudioClip attachAudio;
    [SerializeField] private GameObject attachParticleSystem;
    [SerializeField] private GameObject lingerParticleSystem;


    private AudioSource audioSource;
    private Rigidbody rigBody;

    private void Awake()
    {
        lingerParticleSystem.SetActive(true);
        attachParticleSystem.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        rigBody = GetComponent<Rigidbody>();

        audioSource.clip = attachAudio;
    }

    public void PickUp(GameObject a_pickUpPosition)
    {
        // Parent the pickup to the position passed though
        gameObject.transform.parent = a_pickUpPosition.transform;

        // Disable the lingering particle effect
        lingerParticleSystem.SetActive(false);

        // Disable the RBody
        rigBody.isKinematic = false;
    }

    public void AttachToPlatform()
    {
        attachParticleSystem.SetActive(true);
        audioSource.PlayOneShot(attachAudio);

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        while(audioSource.isPlaying)
        {
            yield return null; 
        }

        Destroy(gameObject);
    }

    public void Drop()
    {
        gameObject.transform.parent = null;
        rigBody.isKinematic = false;
    }
}
