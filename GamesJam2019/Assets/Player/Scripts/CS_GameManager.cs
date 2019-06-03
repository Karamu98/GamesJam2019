using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_GameManager : MonoBehaviour
{

    private bool[] bPlayersIn;
    private int iNumOfPlayers;
    [SerializeField]
    private GameObject[] playerPrefab;
    [SerializeField]
    private GameObject Spawn;

    // Start is called before the first frame update
    void Start()
    {
        iNumOfPlayers = 0;
        bPlayersIn = new bool[4];
        for(int i = 0; i < 4; ++i)
        {
            bPlayersIn[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ListenForPlayer();
    }

    private void ListenForPlayer()
    {
        if (Input.GetButtonDown("A1") && !bPlayersIn[0])
        {
            bPlayersIn[0] = true;
            GameObject player = Instantiate(playerPrefab[0]);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(1);
            player.GetComponent<CS_PlayerController>().SetSpawn(Spawn);
        }
        if (Input.GetButtonDown("A2") && !bPlayersIn[1])
        {
            bPlayersIn[1] = true;
            GameObject player = Instantiate(playerPrefab[1]);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(2);
            player.GetComponent<CS_PlayerController>().SetSpawn(Spawn);

        }

    }
}
