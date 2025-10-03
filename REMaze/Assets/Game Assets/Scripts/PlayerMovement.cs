using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // physics based movement (as the characters will be ragdolls)

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [SerializeField] private float currentVelocity;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float quaternionSpeed;
    [SerializeField] private float quaternionChangeRate;
    private float horizontalInput;
    private float verticalInput;

    [Header("Movement Snapping Settings")]

    public float groundedOffset;

    [Header("Components")]

    private Rigidbody rb;
    public Vector3 moveDirection;

    [Header("Player Actions")]

    public PlayerActions currentAction;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, -gravity, 0); // setting the global gravity for rigidbody
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Update()
    {
        moveDirection = transform.forward * currentVelocity * verticalInput; // accessing the forward direction of the player only using horizontal input
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z); // the y axis shall remain unaffected by the moveDirection as it is affected by jumping

        float turn = horizontalInput * quaternionSpeed * Time.deltaTime; // calculating the turn amount based on horizontal input
        Quaternion rotOffset = Quaternion.Euler(0, turn, 0); // creating a quaternion for the turn amount by converting it to euler angles (y and w)
        rb.MoveRotation(rb.rotation * rotOffset); // move the rotation by multiplying the current rotation with the offset each frame so it rotates smoothly

        MovementControls();
    }

    void MovementControls()
    {
        if(horizontalInput == 0 && verticalInput == 0)
        {
            currentAction = PlayerActions.Idle;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            currentAction = PlayerActions.Sprinting;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentAction = PlayerActions.Dashing;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentAction = PlayerActions.Jumping;
        }
        else
        {
            currentAction = PlayerActions.Walking;
        }

        switch (currentAction)
        {
            case PlayerActions.Idle:
                currentVelocity = Mathf.Lerp(currentVelocity, 0, Time.deltaTime * speedChangeRate); // interpolation. interpolating current velocity to 0 within time amount of time.deltatime (0.156f) and speed change rate
                break;

            case PlayerActions.Walking:
                currentVelocity = Mathf.Lerp(currentVelocity, walkSpeed, Time.deltaTime * speedChangeRate);
                break;

            case PlayerActions.Sprinting:
                currentVelocity = Mathf.Lerp(currentVelocity, sprintSpeed, Time.deltaTime * speedChangeRate);
                break;

            case PlayerActions.Dashing:
                currentVelocity = Mathf.Lerp(currentVelocity, dashSpeed, Time.deltaTime * speedChangeRate);
                break;

            case PlayerActions.Jumping:
                if (isGrounded())
                {
                    rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                }
                break;
        }
    }

    public bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, groundedOffset); // casting a ray from origin to vector3.down and checking if it hits the ground. required for jumping
    }

    private void OnDrawGizmos() // optional: visualize isGrounded() on the editor
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundedOffset);
    }
}

public enum PlayerActions
{
    Idle,
    Walking,
    Sprinting,
    Dashing,
    Jumping
}