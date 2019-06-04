using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager sessionManager;

    private void Awake()
    {
        // Singleton pattern
        if(sessionManager == null)
        {
            sessionManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when all players are dead/when raft sinks
    public static void OnPlayerLose()
    {
        // Save score to highscores

        sessionManager.StartCoroutine(LoseGame()); // Needs a gameobject to start coroutine
    }

    private static IEnumerator LoseGame()
    {
        // Play sad music and shit

        // Wait for sounds and a bit for dramatic effect
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("MainMenu");
    }
}
