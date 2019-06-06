using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    // Cache
    Rigidbody rBody;
    Collider col;
    public bool isPickedUp = false;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void HoldItem(bool a_isHolding)
    {
        if(a_isHolding)
        {
            isPickedUp = true;
            rBody.isKinematic = true;
            col.enabled = false;
        }
        else
        {
            isPickedUp = false;
            rBody.isKinematic = false;
            col.enabled = true;
        }
    }
}
