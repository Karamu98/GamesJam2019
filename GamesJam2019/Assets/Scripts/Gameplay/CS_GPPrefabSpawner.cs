using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GPPrefabSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int m_iTowerHeight;

    [SerializeField]
    private float m_fSpeed;


    [SerializeField]
    private float m_fReplaceDistance;
    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> m_lgoPrefabList = new List<GameObject>();

    [Header("References")]
    [SerializeField]
    private Transform m_tTowerBasePosition;

    [SerializeField]
    private Transform m_tTowerContainer;

    private Queue<Transform> m_qtTransformsQueue;
    private List<GameObject> m_lgoObjectList;

    // Start is called before the first frame update
    void Start()
    {
        m_qtTransformsQueue = new Queue<Transform>();
        m_lgoObjectList = new List<GameObject>();
        SpawnTower();

        m_fReplaceDistance = (m_lgoPrefabList[0].transform.localScale.y * 0.5f) * 2.0f;
    }


    private void SpawnTower()
    {
        for (int i = 0; i < m_iTowerHeight; i++)
        {
            GameObject goTowerPiece = Instantiate(m_lgoPrefabList[Random.Range(0, m_lgoPrefabList.Count)]);
            goTowerPiece.transform.position = m_tTowerBasePosition.position;
            goTowerPiece.transform.position += Vector3.up * (goTowerPiece.transform.localScale.y * 0.5f) * i;

            goTowerPiece.transform.SetParent(m_tTowerContainer);

            m_qtTransformsQueue.Enqueue(goTowerPiece.transform);
            m_lgoObjectList.Add(goTowerPiece);
        }
    }

    private void MoveTower()
    {
        foreach (GameObject goTowerPiece in m_lgoObjectList)
        {
            goTowerPiece.transform.position += Vector3.down * m_fSpeed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        MoveTower();
        ReplaceDistanceCheck();
    }

    private void ReplaceDistanceCheck()
    {
        if(m_qtTransformsQueue.Peek().position.y <= -m_fReplaceDistance)
        {
            Transform goTowerPiece = m_qtTransformsQueue.Dequeue();
            goTowerPiece.position += Vector3.up * m_iTowerHeight * (goTowerPiece.transform.localScale.y * 0.5f);
            m_qtTransformsQueue.Enqueue(goTowerPiece);
        }
    }
}
