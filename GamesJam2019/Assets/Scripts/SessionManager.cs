using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager sessionManager;
    private bool shouldLoadScene = false;

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

        Debug.Log("Game Over");
        sessionManager.StartCoroutine(sessionManager.Wait()); // Needs a gameobject to start coroutine
    }

    private void Update()
    {
        if(shouldLoadScene)
        {
            SceneManager.LoadScene("Menu");
            shouldLoadScene = false;
        }
    }

    private IEnumerator Wait()
    {
        // Play sad music and shit

        // Wait for sounds and a bit for dramatic effect
        yield return new WaitForSeconds(3);

        shouldLoadScene = true;
        StopAllCoroutines();
    }
}
