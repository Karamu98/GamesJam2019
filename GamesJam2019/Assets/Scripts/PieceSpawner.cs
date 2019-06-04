using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private float spawnSepearation = 2.0f;
    [SerializeField] private List<GameObject> piecesToSpawn;
    [SerializeField] private float pieceSpawnChance;
    [SerializeField] private List<GameObject> spawnableObjects;
    [SerializeField] private int piecesAtOnce = 3;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private GameObject playerPlatform;
    private GameObject spawnerArea;

    private float timer;

    // Bounds Checking for spawning
    Vector3 innerXBounds; // min, max, width
    Vector3 outerXBounds;
    float upperRangeX;

    Vector3 innerYBounds;
    Vector3 outerYBounds;
    float upperRangeY;


    private void Awake()
    {
        playerPlatform = FindObjectOfType<PlayerPlatform>().gameObject;
        spawnerArea = FindObjectOfType<Water>().gameObject;


        /// X bound setup
        innerXBounds.x = playerPlatform.GetComponent<Collider>().bounds.min.x; // Min
        innerXBounds.y = playerPlatform.GetComponent<Collider>().bounds.max.x; // Max
        innerXBounds.z = innerXBounds.y - innerXBounds.x; // Width

        outerXBounds.x = spawnerArea.GetComponent<Collider>().bounds.min.x;
        outerXBounds.y = spawnerArea.GetComponent<Collider>().bounds.max.x;
        outerXBounds.z = outerXBounds.y - outerXBounds.x;

        upperRangeX = ((innerXBounds.z * 0.5f) + (outerXBounds.z * 0.5f) + outerXBounds.x);

        /// Y bound setup
        innerYBounds.x = playerPlatform.GetComponent<Collider>().bounds.min.z; // Min
        innerYBounds.y = playerPlatform.GetComponent<Collider>().bounds.max.z; // Max
        innerYBounds.z = innerYBounds.y - innerYBounds.x; // Width

        outerYBounds.x = spawnerArea.GetComponent<Collider>().bounds.min.z;
        outerYBounds.y = spawnerArea.GetComponent<Collider>().bounds.max.z;
        outerYBounds.z = outerYBounds.y - outerYBounds.x;

        upperRangeY = ((innerYBounds.z * 0.5f) + (outerYBounds.z * 0.5f) + outerYBounds.x);
    }

    private void SpawnObjects()
    {
        for(int i = 0; i < piecesAtOnce; i++)
        {
            GameObject newObj;
            Vector2 randPos = Vector2.zero;

            randPos.x = Random.Range(outerXBounds.x, upperRangeX);

            if (randPos.x > (upperRangeX * 0.5f))
            {
                randPos.x += upperRangeX;
            }

            randPos.y = Random.Range(outerYBounds.x, upperRangeY);

            if (randPos.y > (upperRangeY * 0.5f))
            {
                randPos.y += upperRangeY;
            }


            float randObj = Random.Range(0, 100);

            if (randObj <= pieceSpawnChance)
            {
                Debug.Log("PieceSpawn");
                // Spawn boat piece
                int randDebris = Random.Range(0, piecesToSpawn.Count);
                newObj = Instantiate(piecesToSpawn[randDebris]);
            }
            else
            {
                // Spawn debris
                int randDebris = Random.Range(0, spawnableObjects.Count);
                newObj = Instantiate(spawnableObjects[randDebris]);
            }

            newObj.transform.position = new Vector3(randPos.x, gameObject.transform.position.y, randPos.y);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            timer = spawnSepearation;
            SpawnObjects();
        }
    }

}
