using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private int healthToAdd;
    [SerializeField] private AudioClip attachAudio;
    [SerializeField] private GameObject attachParticleSystem;
    [SerializeField] private GameObject lingerParticleSystem;


    private AudioSource audioSource;
    private Rigidbody rigBody;

    private void Awake()
    {
        if(lingerParticleSystem != null)
        {
            lingerParticleSystem.SetActive(true);
        }
        
        if(attachParticleSystem != null)
        {
            attachParticleSystem.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        rigBody = GetComponent<Rigidbody>();

        if (attachAudio != null)
        {
            audioSource.clip = attachAudio;
        }
    }

    /// <summary>
    /// Allow the object to be picked up
    /// </summary>
    /// <param name="a_pickUpPosition">The gameobject this will attach to</param>
    public void PickUp(GameObject a_pickUpPosition)
    {
        // Parent the pickup to the position passed though
        gameObject.transform.parent = a_pickUpPosition.transform;

        // Disable the lingering particle effect
        if(lingerParticleSystem != null)
        {
            lingerParticleSystem.SetActive(false);
        }

        // Disable the RBody
        rigBody.isKinematic = false;
    }

    public void AttachToPlatform()
    {
        if(attachParticleSystem != null)
        {
            attachParticleSystem.SetActive(true);
        }
        else
        {
            Debug.Log(gameObject.name + ": has no attaching particle effect.");
        }

        if(attachAudio != null)
        {
            audioSource.PlayOneShot(attachAudio);
        }
        else
        {
            Debug.Log(gameObject.name + ": has no attaching sound effect.");
        }

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
        if(lingerParticleSystem != null)
        {
            lingerParticleSystem.SetActive(true);
        }
        else
        {
            Debug.Log(gameObject.name + ": has no lingering sound effect.");
        }

        gameObject.transform.parent = null;
        rigBody.isKinematic = false;
    }
}
