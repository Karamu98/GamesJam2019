using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_AISpawner : MonoBehaviour
{
    private enum eGAMESTATES
    {
        MIDROUND,
        ENDROUND,
    }

    [Header("Settings")]
    [SerializeField]
    private int m_iMaxEnemies = 10;

    private int m_iCurrentAgents;
    private int m_iSpawnedAgents;

    [SerializeField]
    private float m_fTimeBetweenSpawns = 5.0f;

    private float m_fTimeSinceLastSpawn;

    [Header("Round Settings")]
    [SerializeField]
    private eGAMESTATES m_eCurrentGameState;

    [SerializeField]
    private int m_iBaseNumberOfAgentsPerRound;

    [SerializeField]
    private float m_fSpawnMultiplier;

    [SerializeField]
    private int m_iCurrentRound;

    [SerializeField]
    private float m_fMaxAgents;

    [SerializeField]
    private int m_fMaxAgentsAllowedOnMap;

    [Header("Agent Settings")]
    [SerializeField]
    private float m_fBaseAgentSpawnDelay;

    [SerializeField]
    private float m_fDelayDecayMultiplier;

    [SerializeField]
    private float m_fSpawnDelay;

    [Header("Enemy Prefabs")]
    [SerializeField]
    private List<GameObject> m_lgoEnemyPrefabList = new List<GameObject>();

    [Header("References")]
    [SerializeField]
    private Text m_tRoundText;

    [SerializeField]
    private List<GameObject> m_lgoSpawnPoints = new List<GameObject>();
    // Start is called before the first frame update
    private void Start()
    {
        m_eCurrentGameState = eGAMESTATES.ENDROUND;
        m_fSpawnDelay = m_fBaseAgentSpawnDelay;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_eCurrentGameState == eGAMESTATES.MIDROUND)
        {
            MidRoundFunction();
        }
        else if (m_eCurrentGameState == eGAMESTATES.ENDROUND)
        {
            EndRoundFunction();
        }
    }

    private void EndRoundFunction()
    {
        m_eCurrentGameState = eGAMESTATES.MIDROUND;//only run once, set gamestate
        m_iCurrentRound += 1;
        m_tRoundText.text = "Round: " + m_iCurrentRound.ToString();

        m_iMaxEnemies = (int)(Mathf.Pow(m_iCurrentRound, 2) * m_fSpawnMultiplier) + m_iBaseNumberOfAgentsPerRound;//Calculate new zombie amount for next round

        m_fSpawnDelay = Mathf.Clamp((m_fBaseAgentSpawnDelay - (Mathf.Pow(m_iCurrentRound, m_fDelayDecayMultiplier))), 0.5f, 100.0f);//Calculate new zombie spawn delay

        m_iSpawnedAgents = 0;
    }

    private void MidRoundFunction()
    {
        m_iCurrentAgents = FindObjectsOfType<CS_AIBase>().Length;
        if (CheckSpawnTimer() &&
            (m_iCurrentAgents < m_iMaxEnemies) &&
            (m_iCurrentAgents < m_fMaxAgentsAllowedOnMap) &&
            (m_iSpawnedAgents <= m_iMaxEnemies))
        {
            SpawnAgent();
        }
        else
        {
            m_fTimeSinceLastSpawn += Time.deltaTime;
        }

        if (m_iSpawnedAgents >= m_iMaxEnemies && m_iCurrentAgents <= 0)
        {
            m_eCurrentGameState = eGAMESTATES.ENDROUND;
        }
        //m_tRoundText.text = m_iCurrentRound.ToString();
    }

    public void SubtractFromAgentCount()
    {
        m_iCurrentAgents -= 1;
    }

    private bool CheckSpawnTimer()
    {
        if (m_fTimeSinceLastSpawn >= m_fSpawnDelay)
        {
            m_fTimeSinceLastSpawn = 0;
            return true;
        }
        return false;
    }

    private void SpawnAgent()
    {
        GameObject goZombie = Instantiate(m_lgoEnemyPrefabList[Random.Range(0, m_lgoEnemyPrefabList.Count)]);
        goZombie.transform.position = m_lgoSpawnPoints[Random.Range(0, m_lgoSpawnPoints.Count)].transform.position;
        m_iCurrentAgents += 1;
        m_iSpawnedAgents += 1;
    }
}