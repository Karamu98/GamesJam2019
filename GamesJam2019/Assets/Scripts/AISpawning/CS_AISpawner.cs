using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AISpawner : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private int m_iMaxEnemies = 10;
    private int m_iCurrentlySpawnedEnemies = 0;

    [SerializeField]
    private float m_fTimeBetweenSpawns = 5.0f;



    [Header("Enemy Prefabs")]
    [SerializeField]
    private List<GameObject> m_lgoEnemyPrefabList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
