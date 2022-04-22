using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This class handles the basic, phsyics based, player movement. Many of the variables should be
 * specified in the editor.
 */
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    /** Value for speed to apply to the player */
    public float moveSpeed = 4.7f;
    /** Multiplier to overcome drag for player speed */
    public float movementMultiplier = 10f;
    /** Multiplier to reduce movement speed in air */
    public float airMultiplier = .24f;
    /** Value obtained in GetInput() for the horizontal player movement */
    private float horizontalMovement;
    /** Value obtained in GetInput() for the Vertical player movement */
    private float verticalMovement;
    /** Direction the player is facing, calculated in GetInput() */
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    [Header("Jumping")]
    /** Force for jumping, set in editor */
    [SerializeField] private float jumpForce = 14;
    /** Multiplier for gravity. Drag is used to limit X and Y movement in air, may need extra gravity because of this. */
    [SerializeField] private float gravityMultiplier = 2.6f;
    private bool jumped;

    [Header("Drag")]
    /** Ammount of drag to be applied to player movement (ground higher than air) */
    public float groundDrag = 6f;
    public float airDrag = 1.5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    /** Distance from ground that counts as touching it */
    [SerializeField] private float groundDistance = .4f;
    /** Mask containing only the ground */
    [SerializeField] LayerMask groundMask;
    RaycastHit slopeHit;

    [Header("Player info")]
    /** Set to player's height */
    public float playerHeight;
    /** Empty game object for keeping track of orientation attached to the player */
    [SerializeField] private Transform orientation;
    /** Rigidbody of the player */
    public Rigidbody player;
    private bool paused;
    private int jumpsLeft = 2;

    void Start()
    {
        player.useGravity = false;
        player.freezeRotation = true;
        moveSpeed = 4.7f;
        movementMultiplier = 10f;
        airMultiplier = .24f;
        jumpForce = 14;
        gravityMultiplier = 2.6f;
        groundDrag = 6;
        airDrag = 1.5f;
        groundDistance = .4f;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //GetInput();
        ControlDrag();
        if (isGrounded)
        {
            //frogs
            jumpsLeft = 1;
        }
        if (jumped && isGrounded)
        {
            Jump();
        } else if (jumped && jumpsLeft > 0)
        {
            jumpsLeft--;
            Jump();
        }
        if (paused)
        {
            GameManager.instance.PauseGame();
            paused = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    private void Jump()
    {
        jumped = false;
        player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
        player.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            player.drag = groundDrag;
        } else
        {
            player.drag = airDrag;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
        
    }

    void FixedUpdate()
    {
        CalculateGravity();
        MovePlayer();
    }

    /**
     * Calculates which type of gravity to use. Used to prevent player from slipping down slopes.
     */
    void CalculateGravity()
    {
        if (!OnSlope())
        {
            player.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        } else
        {
            player.AddForce(-slopeHit.normal * Physics.gravity.magnitude, ForceMode.Acceleration);
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (isGrounded && !OnSlope())
        {
            Debug.Log("1 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            player.AddForce(movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        } else if (isGrounded)
        {
            Debug.Log("2 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            player.AddForce(moveSpeed * movementMultiplier * slopeMoveDirection.normalized, ForceMode.Acceleration);
        } else
        {
            Debug.Log("3 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            player.AddForce(airMultiplier * movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        }
        
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * .5f + .5f)) {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        paused = context.action.triggered;
    }
}
