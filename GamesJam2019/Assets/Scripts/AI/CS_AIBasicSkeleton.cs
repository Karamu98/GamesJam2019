using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AIBasicSkeleton : CS_AIBase
{
    // Start is called before the first frame update
    void Start()
    {
        InitialiseAgent();
        ChooseNewTarget();
    }

    
}
