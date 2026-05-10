using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private AudioSource moveAudioSource;
    [SerializeField] private AudioSource jumpAudioSource;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private float verticalVelocity;
    private bool isGrounded;
    private bool jumped = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) { return; }

        // ── Ground detection via sphere cast (works on any surface, not just y=0) ──
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset downward velocity when grounded so we don't accumulate infinite gravity
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // small negative keeps CheckSphere reliable
        }

        // ── Horizontal movement (unchanged input logic) ──
        Vector3 inputVect = gameInput.GetMovementVector().normalized;
        Vector3 moveDir = (transform.right * inputVect.x + transform.forward * inputVect.z) * moveSpeed;

        bool isMoving = (inputVect.x != 0 || inputVect.z != 0);

        // ── Footstep audio (unchanged) ──
        if (isMoving && isGrounded)
        {
            if (!moveAudioSource.isPlaying) moveAudioSource.Play();
        }
        else
        {
            if (moveAudioSource.isPlaying) moveAudioSource.Stop();
        }

        // ── Jump ──
        if (isGrounded && gameInput.IsJumpPressed())
        {
            jumped = true;
            // v = sqrt(jumpForce * -2 * gravity) gives predictable jump height
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // ── Landing audio (plays when descending near ground) ──
        if (isGrounded && verticalVelocity <= 0f && jumped)
        {
            if (!jumpAudioSource.isPlaying) jumpAudioSource.Play();
            jumped = false;
        }

        // ── Apply gravity ──
        verticalVelocity += gravity * Time.deltaTime;

        // ── Move via CharacterController (collision-safe) ──
        Vector3 finalMovement = moveDir;
        finalMovement.y = verticalVelocity;
        controller.Move(finalMovement * Time.deltaTime);
    }
}
