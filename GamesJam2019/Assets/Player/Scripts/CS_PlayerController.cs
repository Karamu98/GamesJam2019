using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_PlayerController : MonoBehaviour, IDamageable
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float meleeSpeed = 0.05f;
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float meleeAngle = 45;
    [SerializeField] private float rangeAttackSpeed = 0.2f;
    [SerializeField] private int rangedDamage = 10;
    [SerializeField] private float knockDownTime = 1;


    private int iPlayerNum;
    private Rigidbody nav;
    private bool bStunned;
    private float knockDownTimer;
    public bool bInvunerable;

    [Header("Spawnables")]
    [SerializeField]
    private GameObject arrowPrefab;

    private bool bCanRangeAttack = true;
    private float fTimerForRange = 0;
    private float fDeathTimer = 3;
    private bool bInWater = false;
    private bool bCarrying = false;
    private bool bJumping = false;
    private bool bPickUp = false;

    [SerializeField]
    private GameObject g;
    Vector3 LookTo;

    private GameObject spawnPoint;
    [SerializeField]
    private GameObject pickUpPos;
    private GameObject ObjectPickedUp;

    private Vector3 origBoxCentre;


    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<Rigidbody>();
        bStunned = false;
        knockDownTimer = knockDownTime;
        LookTo = new Vector3(100, 0, 0);
        origBoxCentre = GetComponent<BoxCollider>().center;
        bInvunerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!bStunned )
        {
            PlayerMovement();
            if(Input.GetButtonDown("Melee" + iPlayerNum) && !bPickUp)
            {
                MeleeAttack();
            }
            else if(Input.GetAxis("Range" + iPlayerNum) == 1 && !bPickUp)
            {
                RangeAttack();
            }
            if (Input.GetButtonDown("A" + iPlayerNum) && !bJumping)
            {
                //Jump
                gameObject.GetComponent<Animator>().SetBool("bJump", true);
                gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bJump", true);
                bJumping = true;
                SetBoxCentre(new Vector3(0, 0.023f, 0));
                GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(WaitForJump());
            }
            if (Input.GetButtonDown("B" + iPlayerNum) && !bJumping)
            {
                if (!bPickUp)
                {
                    if(CheckForObject())
                    {
                        //PickUp
                        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bPickUp", true);
                        bPickUp = true;
                        Debug.Log("PickedUP");
                    }

                }
                else
                {
                    //Drop
                    gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bPickUp", false);
                    bPickUp = false;
                    CheckForRaft();
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
                fTimerForRange = rangeAttackSpeed;
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
        SetBoxCentre(Vector3.zero);
        GetComponent<Rigidbody>().useGravity = true;

    }

    private void CheckForRaft()
    {
        GameObject Raft = FindObjectOfType<PlayerPlatform>().gameObject;
        if(Vector3.Distance(gameObject.transform.position, Raft.transform.position) < 1.5f)
        {
            ObjectPickedUp.GetComponent<PlatformPiece>().AttachToPlatform();
        }
        else
        {
            ObjectPickedUp.GetComponent<PlatformPiece>().Drop();
        }
    }

    private void ResetPlayer()
    {
        gameObject.transform.position = spawnPoint.transform.position;
    }

    private IEnumerator WaitForMelee()
    {
        yield return new WaitForSeconds(meleeSpeed);
        gameObject.GetComponent<Animator>().SetBool("bMelee", false);
        gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bMelee", false);
    }

    private void PlayerMovement()
    {
        nav.velocity = new Vector3(Input.GetAxis("Horizontal" + iPlayerNum) * moveSpeed,
         0, Input.GetAxis("Vertical" + iPlayerNum) * moveSpeed);

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
            if (angle > -meleeAngle && angle < meleeAngle)
            {
                ai.DamageAgent(meleeDamage, gameObject);
            }
        }
        StartCoroutine(WaitForMelee());
    }

    private bool CheckForObject()
    {
        PlatformPiece[] debris = FindObjectsOfType<PlatformPiece>();
        foreach (PlatformPiece deb in debris)
        {
            //float angle = Vector3.Angle(gameObject.transform.forward, gameObject.transform.position - deb.transform.position);
            //if (angle > -meleeAngle && angle < meleeAngle)
            //{
            if(Vector3.Distance(gameObject.transform.position, deb.gameObject.transform.position) <= 1.5f)
            { 
                //PickUp
                deb.PickUp(pickUpPos);
                ObjectPickedUp = deb.gameObject;
                return true;
            }
        }
        return false;
    }

    private void RangeAttack()
    {
        if (bCanRangeAttack)
        {
            AttackEnemy();
            bCanRangeAttack = false;
        }
    }

    private void SetBoxCentre(Vector3 a_Add)
    {
        gameObject.GetComponent<BoxCollider>().center = origBoxCentre + a_Add;

    }

    private void AttackEnemy()
    {
        GameObject goProjectile = Instantiate(arrowPrefab);
        goProjectile.transform.position = transform.GetChild(2).transform.position;
        goProjectile.transform.position -= transform.GetChild(2).transform.forward * 2.0f;
        goProjectile.GetComponent<CS_Arrow>().Initialise(gameObject.transform.GetChild(2).transform, rangedDamage, gameObject);
    }

    public void SetPlayerNumber(int a_iNum)
    {
        iPlayerNum = a_iNum;
    }

    public void SetSpawn(GameObject a_Spawn)
    {
        spawnPoint = a_Spawn;
    }

    private void PassOut()
    {
        knockDownTimer -= Time.deltaTime;
        if(knockDownTimer <= 0)
        {
            bStunned = false;
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bFall", false);
            knockDownTimer = 1;
        }
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

    public void TakeDamage(int a_damageToTake, GameObject a_instigator)
    {
        if (!bStunned && !bInvunerable)
        {
            bStunned = true;
            gameObject.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("bFall", true);
            bInvunerable = true;
            StartCoroutine(IfInvnuerable());
        }
    }

    private IEnumerator IfInvnuerable()
    {
        yield return new WaitForSeconds(2);
        bInvunerable = false;
    }

    public void Heal(int a_healthToHeal, GameObject a_instigator)
    {
        throw new System.NotImplementedException();
    }
}
