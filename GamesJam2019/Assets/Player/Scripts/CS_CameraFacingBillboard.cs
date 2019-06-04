using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraFacingBillboard : MonoBehaviour
{
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(cam.transform);
    }
}
