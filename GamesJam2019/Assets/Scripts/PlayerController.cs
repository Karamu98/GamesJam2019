using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerStates : byte
{
    None = 0,
    CanFireWeapon = 1,
    InWater = 2,
    IsCarrying = 4,
    IsJumping = 8

}

public class PlayerController : MonoBehaviour, IDamageable
{
    private static byte playerCount = 0;


    [Header("Inspector References")]
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private GameObject feetObject;
    [SerializeField] private GameObject itemLocation;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject gunPosition;

    [Header("Player Settings")]
    [SerializeField] private float playerGracePeriod = 5;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float meleeSpeed = 0.05f;
    [SerializeField] private int meleeDamage = 50;
    [SerializeField] private float meleeAngle = 45;
    [SerializeField] private float rangeAttackSpeed = 0.2f;
    [SerializeField] private int rangedDamage = 1000;
    [SerializeField] private float knockDownTime = 1;
    [SerializeField] private float jumpPower = 3;

    [Header("Controls")]
    [SerializeField] private XboxControllerButton shootButton = XboxControllerButton.RT;
    [SerializeField] private XboxControllerButton meleeButton = XboxControllerButton.RB;
    [SerializeField] private XboxControllerButton jumpButton = XboxControllerButton.LT;
    [SerializeField] private XboxControllerButton pickupButton = XboxControllerButton.LB;


    // State control
    private bool canRangeAttack = false;
    private bool canMeleeAttack = false;
    private bool isStunned = false;
    private bool isInWater = false;
    private bool isCarrying = false;
    private bool isOnGround = false;
    private int health;

    // Cache
    private Rigidbody rBody;
    private byte playerNumber;
    private Vector3 moveVelocity = Vector3.zero;
    private float floorDist;
    GameObject pickedUpItem = null;
    Vector3 startPos;
    private PickUpVolume pickUpVolume;

    // Timers
    private float rangeAttackTimer = 0;

    private void Awake()
    {
        // What player is this
        playerNumber = playerCount;
        playerCount++;

        rBody = GetComponent<Rigidbody>();

        Bounds bounds = GetComponentInChildren<Collider>().bounds;
        floorDist = bounds.center.y - bounds.min.y;

        health = maxHealth;

        startPos = transform.position;
        pickUpVolume = GetComponentInChildren<PickUpVolume>();
    }

    private void Update()
    {
        PlayerUpdate();

        if (isStunned)
        {
            return;
        }

        moveVelocity = Vector3.zero;
        HandleMovement();
        HandleJumping();
        HandleWeapons();
        HandlePickup();
    }

    private void FixedUpdate()
    {
        if(moveVelocity.y > 0)
        {
            rBody.AddForce(Vector3.up * jumpPower);
        }

        rBody.velocity = new Vector3(moveVelocity.x, rBody.velocity.y, moveVelocity.z);

        moveVelocity = Vector3.zero;
    }

    public static byte GetPlayerCount()
    {
        return playerCount;
    }

    public byte GetPlayerNumber()
    {
        return playerNumber;
    }

    private void Shoot()
    {
        GameObject newProj = Instantiate(projectile);
        newProj.transform.position = gunPosition.transform.position + -gunPosition.transform.forward * 2;
        newProj.transform.forward = -bodyObject.transform.forward;

        newProj.GetComponent<Arrow>().Initialise(rangedDamage, gameObject);

        canRangeAttack = false;
        rangeAttackTimer = rangeAttackSpeed;
    }

    private bool IsOnGround()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - floorDist, transform.position.z), Color.green);
        if(Physics.Raycast(transform.position, -Vector3.up, floorDist))
        {
            isOnGround = true;
            return true;
        }
        else
        {
            isOnGround = false;
            return false;
        }
    }

    private void HandleJumping()
    {
        if(IsOnGround())
        {
            ;
            if(Input.GetAxisRaw(XboxController.GetButtonName(jumpButton) + playerNumber) > 0)
            {
                moveVelocity.y = 1;
            }
        }
        else
        {
            moveVelocity.y = 0;
        }
    }

    private void HandleMelee()
    {
        if(canMeleeAttack && Input.GetAxisRaw(XboxController.GetButtonName(meleeButton) + playerNumber) > 0.0f)
        {

        }
    }

    private void HandleMovement()
    {
        Vector3 leftStickInput = new Vector3(Input.GetAxisRaw("LHorizontal" + playerNumber), 0, -Input.GetAxisRaw("LVertical" + playerNumber));

        // Movement
        moveVelocity = leftStickInput * moveSpeed;

        // Feet roatation
        Vector3 playerDir = Vector3.right * leftStickInput.x + Vector3.forward * leftStickInput.z;
        if (playerDir.sqrMagnitude > 0.0f)
        {
            feetObject.transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);
        }

        // Body rotation
        playerDir = Vector3.right * Input.GetAxisRaw("RHorizontal" + playerNumber) + Vector3.forward * -Input.GetAxisRaw("RVertical" + playerNumber);
        if(playerDir.sqrMagnitude > 0.0f)
        {
            bodyObject.transform.rotation = Quaternion.LookRotation(-playerDir, Vector3.up);
        }
    }

    private void PlayerUpdate()
    {
        if(!canRangeAttack)
        {
            rangeAttackTimer -= Time.deltaTime;
            if(rangeAttackTimer <= 0.0f)
            {
                canRangeAttack = true;
            }
        }
    }

    private void HandleWeapons()
    {
        if(canRangeAttack && Input.GetAxisRaw(XboxController.GetButtonName(shootButton) + playerNumber) > 0.0f)
        {
            Shoot();
        }
    }

    private void HandlePickup()
    {
        if(Input.GetButtonDown(XboxController.GetButtonName(pickupButton) + playerNumber) && !isCarrying)
        {
            GameObject item = pickUpVolume.PickupItem();
            if(item != null)
            {
                pickedUpItem = item;
                pickedUpItem.transform.parent = itemLocation.transform;
                pickedUpItem.transform.localPosition = Vector3.zero;
                pickedUpItem.GetComponent<PickUpItem>().HoldItem(true);
                isCarrying = true;
            }
            else
            {
                pickedUpItem = null;
                isCarrying = false;
            }
        }
        else if(Input.GetButtonDown(XboxController.GetButtonName(pickupButton) + playerNumber) && isCarrying)
        {
            pickedUpItem.transform.parent = null;
            pickedUpItem.GetComponent<PickUpItem>().HoldItem(false);
            pickedUpItem = null;
            isCarrying = false;
        }
    }

    private void Die()
    {
        transform.position = startPos;
        health = maxHealth;
    }


    #region Interface

    public void Heal(int a_healthToHeal, GameObject a_instigator)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int a_damageToTake, GameObject a_instigator)
    {
        if(a_instigator == gameObject)
        {
            health -= a_damageToTake;
            return;
        }

        PlayerController otherPlayer = a_instigator.GetComponent<PlayerController>();
        if (otherPlayer != null)
        {
            health -= Mathf.FloorToInt(a_damageToTake * 0.5f);
            Debug.Log("PVP: " + a_instigator.name + " delt " + Mathf.FloorToInt(a_damageToTake * 0.5f) + " damage to " + gameObject.name);
        }
        else
        {
            health -= a_damageToTake;
        }

        if(health <= 0)
        {
            Debug.Log("PVP: " + a_instigator.name + " killed " + gameObject.name);
            Die();
        }

    }

    #endregion

}
