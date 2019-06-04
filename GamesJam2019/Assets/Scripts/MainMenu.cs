using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerOneButton;
    [SerializeField] private GameObject playerOneTick;

    [SerializeField] private GameObject playerTwoButton;
    [SerializeField] private GameObject playerTwoTick;

    [SerializeField] private AudioClip selectSound;

    bool[] readyPlayers = new bool[2];


    private void Update()
    {
        if(Input.GetButtonDown("A1") || Input.GetKeyDown(KeyCode.A))
        {
            playerOneButton.GetComponent<Button>().interactable = false;
            playerOneTick.GetComponent<Image>().enabled = true;
            readyPlayers[0] = true;

            if(selectSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(selectSound);
            }
        }

        if (Input.GetButtonDown("A2") || Input.GetKeyDown(KeyCode.L))
        {
            playerTwoButton.GetComponent<Button>().interactable = false;
            playerTwoTick.GetComponent<Image>().enabled = true;
            readyPlayers[1] = true;

            if (selectSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(selectSound);
            }
        }

        bool bothReady = true;
        foreach(bool player in readyPlayers)
        {
            if(!player)
            {
                bothReady = false;
                break;
            }
        }

        if(bothReady)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        // Play sound fx

        // Wait for them to finish
        yield return new WaitForSeconds(3);

        // Load game
        SceneManager.LoadScene("Level");
    }

}
