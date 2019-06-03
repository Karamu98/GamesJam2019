using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_GameManager : MonoBehaviour
{

    private bool[] bPlayersIn;
    private int iNumOfPlayers;
    [SerializeField]
    private GameObject playerPrefab;

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
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(1);
        }
        if (Input.GetButtonDown("A2") && !bPlayersIn[1])
        {
            bPlayersIn[1] = true;
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(2);
        }
        if (Input.GetButtonDown("A3") && !bPlayersIn[2])
        {
            bPlayersIn[2] = true;
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(3);
        }
        if (Input.GetButtonDown("A4") && !bPlayersIn[3])
        {
            bPlayersIn[3] = true;
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<CS_PlayerController>().SetPlayerNumber(4);
        }

    }
}
