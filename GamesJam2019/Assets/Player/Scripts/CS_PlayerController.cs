using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_PlayerController : MonoBehaviour, IDamageable
{
    private int iPlayerNum;
    private Rigidbody nav;
    private float fSpeed;
    private bool bStunned;
    private float fTimer;

    [SerializeField]
    private GameObject arrowPrefab;

    float fMeleeAngle = 45; // 45 degrees, Melee range
    float fMeleeDamage = 10;
    float fRangeDamage = 10;
    private bool bCanRangeAttack = true;
    private float fTimerForRange = 1;
    private float fDeathTimer = 3;
    private bool bInWater = false;
    private bool bCarrying = false;
    private bool bJumping = false;
    private bool bPickUp = false;

    [SerializeField]
    private GameObject g;
    Vector3 LookTo;

    private GameObject Spawn;



    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<Rigidbody>();
        fSpeed = 1f;
        bStunned = false;
        fTimer = 3;
        LookTo = new Vector3(100, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!bStunned && !bCarrying)
        {
            PlayerMovement();
            if(Input.GetButtonDown("Melee" + iPlayerNum))
            {
                MeleeAttack();
            }
            else if(Input.GetAxis("Range" + iPlayerNum) == 1)
            {
                RangeAttack();
            }
            if (Input.GetButtonDown("A" + iPlayerNum) && !bJumping)
            {
                //Jump
                gameObject.GetComponent<Animator>().SetBool("bJump", true);
                gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bJump", true);
                bJumping = true;
                StartCoroutine(WaitForJump());
            }
            if (Input.GetButtonDown("B" + iPlayerNum) && !bJumping)
            {
                if (!bPickUp)
                {
                    //PickUp
                    gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bPickUp", true);
                    bPickUp = true;
                }
                else
                {
                    //Drop
                    gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bPickUp", false);
                    bPickUp = false;
                }

            }
        }
        else if (bStunned)
        {
            PassOut();
        }


        if(bInWater)
        {
            fDeathTimer -= Time.deltaTime;
            if(fDeathTimer <= 0)
            {
                ResetPlayer();
                fDeathTimer = 3;
                bInWater = false;
            }
        }
        if(!bCanRangeAttack)
        {
            fTimerForRange -= Time.deltaTime;
            if(fTimerForRange <= 0)
            {
                fTimerForRange = 1;
                bCanRangeAttack = true;
            }
        }
    }

    private IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(1f);
        bJumping = false;
        gameObject.GetComponent<Animator>().SetBool("bJump", false);
        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bJump", false);
    }

    private void ResetPlayer()
    {
        gameObject.transform.position = Spawn.transform.position;
    }

    private IEnumerator WaitForMelee()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Animator>().SetBool("bMelee", false);
        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bMelee", false);
    }

    private void PlayerMovement()
    {
        nav.velocity = new Vector3(Input.GetAxis("Horizontal" + iPlayerNum) * fSpeed,
         0, Input.GetAxis("Vertical" + iPlayerNum) * fSpeed);

        float fx = -Input.GetAxis("Horizontal" + iPlayerNum);
        Vector3 Look = new Vector3(fx, 0, Input.GetAxis("Vertical" + iPlayerNum));
        Look += gameObject.transform.position;
        gameObject.transform. LookAt(Look);

        if (nav.velocity.magnitude != 0)
        {
            gameObject.GetComponent<Animator>().SetBool("bWalking", true);
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bWalking", true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("bWalking", false);
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bWalking", false);

        }
        CalculateRotation();
    }

    private void CalculateRotation()
    {
        //gameObject.transform.GetChild(1).transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis("RotateHorizontal" + iPlayerNum), Input.GetAxis("RotateVertical" + iPlayerNum)) * 180 / Mathf.PI * 2, 0);
        float fx = -Input.GetAxis("RotateHorizontal" + iPlayerNum);
        float fy = Input.GetAxis("RotateVertical" + iPlayerNum);
        Debug.Log(fx + ", " + fy);
        if(fx != 0 && fy != 0)
        {
            LookTo = new Vector3(fx, 0, fy);
            LookTo += gameObject.transform.GetChild(2).transform.position;
        }
        gameObject.transform.GetChild(2).transform.LookAt(LookTo);
    }

    private void MeleeAttack()
    {
        gameObject.GetComponent<Animator>().SetBool("bWalking", false);
        gameObject.GetComponent<Animator>().SetBool("bMelee", true);
        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bWalking", false);
        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bMelee", true);
        CS_AIBase[] Enemies = FindObjectsOfType<CS_AIBase>();
        foreach(CS_AIBase ai in Enemies)
        {
            float angle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.position - ai.transform.position);
            if (angle > -fMeleeAngle && angle < fMeleeAngle)
            {
                ai.DamageAgent(fMeleeDamage, gameObject);
            }
        }
        StartCoroutine(WaitForMelee());
    }

    private void CheckForObject()
    {
        Debris[] debris = FindObjectsOfType<Debris>();
        foreach (Debris deb in debris)
        {
            float angle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.position - deb.transform.position);
            if (angle > -fMeleeAngle && angle < fMeleeAngle)
            {
                //PickUp
            }
        }
    }

    private void RangeAttack()
    {
        if (bCanRangeAttack)
        {
            AttackEnemy();
            bCanRangeAttack = false;
        }
    }

    private void AttackEnemy()
    {
        GameObject goProjectile = Instantiate(arrowPrefab);
        goProjectile.transform.position = transform.GetChild(2).transform.position;
        goProjectile.transform.position -= transform.GetChild(2).transform.forward * 2.0f;
        goProjectile.GetComponent<CS_Arrow>().Initialise(gameObject.transform.GetChild(2).transform, fRangeDamage, gameObject);
    }

    public void SetPlayerNumber(int a_iNum)
    {
        iPlayerNum = a_iNum;
    }

    public void SetSpawn(GameObject a_Spawn)
    {
        Spawn = a_Spawn;
    }

    private void PassOut()
    {
        fTimer -= Time.deltaTime;
        if(fTimer <= 0)
        {
            bStunned = false;
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bFall", false);
            fTimer = 3;
        }
    }


    public void TakeDamage(int a_damageToTake)
    {
        if (!bStunned)
        {
            bStunned = true;
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bFall", true);
        }
    }

    public void Heal(int a_healthToHeal)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Water>())
        {
            bInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Water>())
        {
            bInWater = false;
        }
    }

}
