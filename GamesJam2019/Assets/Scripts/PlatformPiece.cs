using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    public int healthToAdd;
    public GameObject itemPrefab;
    public AudioClip attachAudio;
    public GameObject attachParticleSystem;
    public GameObject lingerParticleSystem;

    public void Initialise(GameObject a_spawnerPosition)
    {
        GameObject newObj = Instantiate(itemPrefab);
        newObj.transform.parent = a_spawnerPosition.transform;
        newObj.transform.localPosition = Vector3.zero;

        
    }
}
