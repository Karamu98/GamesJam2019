using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_GameManager : MonoBehaviour
{

    private bool[] bPlayersIn;
    private int iNumOfPlayers;
    [SerializeField]
    private GameObject[] playerPrefab;
    [SerializeField] private GameObject[] spawnPoints;

    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        iNumOfPlayers = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        SpawnPlayers(2);
    }

    private void SpawnPlayers(int a_playerCount)
    {
        players = new GameObject[a_playerCount];

        iNumOfPlayers = a_playerCount;

        int playerNum = 0;

        for(int i = 0; i < a_playerCount; i++)
        {
            players[playerNum] = Instantiate(playerPrefab[0]);
            players[playerNum].gameObject.transform.position = spawnPoints[playerNum].gameObject.transform.position;
            players[playerNum].GetComponent<CS_PlayerController>().SetPlayerNumber(playerNum + 1);
            players[playerNum].GetComponent<CS_PlayerController>().SetSpawn(spawnPoints[playerNum]);

            playerNum++;
        }
    }
}
