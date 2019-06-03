using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum  eAIEnemyAnimationStage
{
    AIANIM_RUN,
    AIANIM_ATTACK,
    AIANIM_IDLE
}


public class CS_AIAnimationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject m_goRunRef;

    [SerializeField]
    private GameObject m_goIdleRef;

    [SerializeField]
    private GameObject m_goAttackRef;

    [SerializeField]
    private GameObject[] m_lgoSelfRenderers;

    private eAIEnemyAnimationStage m_eAnimationStage;

    private Animator m_aAttackAnimator;
    private void Start()
    {
        Still();
    }

    private void DisableAllRefs()
    {
        m_goRunRef.SetActive(false);
        m_goIdleRef.SetActive(false);
        m_goAttackRef.SetActive(false);
    }

    private void Update()
    {
        if(m_aAttackAnimator == null)
        {
            return;
        }
        if (m_aAttackAnimator.GetBool("Attack"))
        {
            //m_aAttackAnimator.SetBool("Attack", false);
        }

    }

    public void Run()
    {
        if (m_eAnimationStage != eAIEnemyAnimationStage.AIANIM_ATTACK)
        {
            if (m_eAnimationStage != eAIEnemyAnimationStage.AIANIM_RUN)
            {
                DisableAllRefs();
                m_goRunRef.SetActive(true);
                m_eAnimationStage = eAIEnemyAnimationStage.AIANIM_RUN;
            }

        }

    }
    public void Still()
    {
        if (m_eAnimationStage != eAIEnemyAnimationStage.AIANIM_ATTACK)
        {
            if (m_eAnimationStage != eAIEnemyAnimationStage.AIANIM_IDLE)
            {
                DisableAllRefs();
                m_goIdleRef.SetActive(true);
                m_eAnimationStage = eAIEnemyAnimationStage.AIANIM_IDLE;
            }
        }
    }
    public void Attack()
    {
        if(m_aAttackAnimator == null)
        {
            m_aAttackAnimator = m_goAttackRef.GetComponent<Animator>();
        }



        if (m_eAnimationStage != eAIEnemyAnimationStage.AIANIM_ATTACK)
        {
            m_eAnimationStage = eAIEnemyAnimationStage.AIANIM_ATTACK;

            m_aAttackAnimator.SetBool("Attack", true);
            m_goAttackRef.SetActive(true);
            HideSelfRenderers();
            StartCoroutine(ResetAnimationBool(1.0f));

        }
    }

    private void HideSelfRenderers()
    {
        foreach (GameObject goObject in m_lgoSelfRenderers)
        {
            goObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void ShowSelfRenderers()
    {
        foreach (GameObject goObject in m_lgoSelfRenderers)
        {
            goObject.GetComponent<Renderer>().enabled = true;
        }
    }

    private IEnumerator ResetAnimationBool(float a_fTime)
    {
        yield return new WaitForSeconds(a_fTime);
        m_aAttackAnimator.SetBool("Attack", false);
        ShowSelfRenderers();
        m_goAttackRef.SetActive(false);
        m_eAnimationStage = eAIEnemyAnimationStage.AIANIM_IDLE;


    }
}
