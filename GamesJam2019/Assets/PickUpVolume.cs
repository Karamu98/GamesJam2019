using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpVolume : MonoBehaviour
{
    [SerializeField] GameObject holder;
    private List<GameObject> pickupObjects = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if(pickupObjects.Contains(other.gameObject))
        {
            return;
        }
        PickUpItem pickupItem = other.GetComponent<PickUpItem>();
        if(pickupItem != null)
        {
            pickupObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpItem pickUpItem = other.GetComponent<PickUpItem>();

        if(pickUpItem != null)
        {
            pickupObjects.Remove(other.gameObject);
        }
    }

    public GameObject PickupItem()
    {
        // Clear dead references and test size
        pickupObjects.RemoveAll(item => item == null || item.GetComponent<PickUpItem>().isPickedUp == true);
        if(pickupObjects.Count <= 0)
        {
            return null;
        }

        KeyValuePair<float, GameObject> closestObj = new KeyValuePair<float, GameObject>(99999, null);

        foreach(GameObject obj in pickupObjects)
        {
            if(closestObj.Value == null)
            {
                closestObj = new KeyValuePair<float, GameObject>(Vector3.Distance(holder.gameObject.transform.position, obj.transform.position), obj);
                continue;
            }

            if(Vector3.Distance(holder.gameObject.transform.position, obj.transform.position) < closestObj.Key)
            {
                closestObj = new KeyValuePair<float, GameObject>(Vector3.Distance(holder.gameObject.transform.position, obj.transform.position), obj);
            }
        }

        return closestObj.Value;
    }
}
