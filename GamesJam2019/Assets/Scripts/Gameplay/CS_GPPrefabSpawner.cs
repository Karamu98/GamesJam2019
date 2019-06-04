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
    private int m_iStartDistanceMultiplier;

    [SerializeField]
    private int m_iReplaceDistanceMultiplier;
    [SerializeField]
    private float m_fReplaceDistance;
    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> m_lgoPrefabList = new List<GameObject>();

    [SerializeField]
    private GameObject m_goFloorPiece;

    [Header("References")]
    [SerializeField]
    private Transform m_tTowerBasePosition;

    [SerializeField]
    private Transform m_tTowerContainer;

    private Queue<Transform> m_qtTransformsQueue;
    private List<GameObject> m_lgoObjectList;

    private GameObject m_goFloorRef;
    private bool m_bDeleteFloor;
    // Start is called before the first frame update
    void Start()
    {
        m_qtTransformsQueue = new Queue<Transform>();
        m_lgoObjectList = new List<GameObject>();
        SpawnTower();

        m_fReplaceDistance = (m_lgoPrefabList[0].transform.localScale.y * 0.5f) * m_iReplaceDistanceMultiplier;
    }


    private void SpawnTower()
    {
        for (int i = 0; i < m_iTowerHeight; i++)
        {
            GameObject goTowerPiece = Instantiate(m_lgoPrefabList[Random.Range(0, m_lgoPrefabList.Count)]);
            goTowerPiece.transform.position = m_tTowerBasePosition.position;
            goTowerPiece.transform.position += Vector3.up * (goTowerPiece.transform.localScale.y * 0.5f) * i;

            goTowerPiece.transform.SetParent(m_tTowerContainer);

            if(i == 0)
            {
                m_goFloorRef = Instantiate(m_goFloorPiece, goTowerPiece.transform);

            }

            m_qtTransformsQueue.Enqueue(goTowerPiece.transform);
            m_lgoObjectList.Add(goTowerPiece);
        }

        //foreach (GameObject goTowerPiece in m_lgoObjectList)
        //{
        //    goTowerPiece.transform.position += Vector3.down * m_iStartDistanceMultiplier;
        //}
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
            if(!m_bDeleteFloor)
            {
                Destroy(m_goFloorRef);
            }
        }
    }
}
