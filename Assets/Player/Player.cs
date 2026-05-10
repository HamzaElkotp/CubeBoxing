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
        if (!GameManager.Instance.IsGamePlaying()) {
            if (moveAudioSource.isPlaying) moveAudioSource.Stop();
            if (jumpAudioSource.isPlaying) jumpAudioSource.Stop();
            return; 
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; 
        }

        Vector3 inputVect = gameInput.GetMovementVector().normalized;

        bool isMoving = (inputVect.x != 0 || inputVect.z != 0);

        if (isMoving && isGrounded)
        {
            if (!moveAudioSource.isPlaying) moveAudioSource.Play();
        }
        else
        {
            if (moveAudioSource.isPlaying) moveAudioSource.Stop();
        }

        if (isGrounded && gameInput.IsJumpPressed())
        {
            jumped = true;
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (isGrounded && verticalVelocity <= 0f && jumped)
        {
            if (!jumpAudioSource.isPlaying) jumpAudioSource.Play();
            jumped = false;
        }

        verticalVelocity += gravity * Time.deltaTime;

        HandleHit();
        float currentSpeed = isHitting
            ? moveSpeed * Mathf.Lerp(1f, hitSpeedMultiplier, hitBoostCurve)
            : moveSpeed;

        Vector3 moveDir = (transform.right * inputVect.x + transform.forward * inputVect.z) * currentSpeed;

        Vector3 finalMovement = moveDir;
        finalMovement.y = verticalVelocity;
        controller.Move(finalMovement * Time.deltaTime);
    }


    [Header("Hit Settings")]
    [SerializeField] private float hitSpeedMultiplier = 2.5f;
    [SerializeField] private float hitForce           = 18f;
    [SerializeField] private float hitDuration        = 0.5f;
    [SerializeField] private float hitCooldown        = 3f; 
    [SerializeField] private AudioSource hitAudioSource;

    private bool  isHitting    = false;
    private float hitTimer     = 0f;
    private float lastHitTime  = -999f;
    private float hitBoostCurve = 0f;

    public float HitProgress => isHitting ? hitTimer / hitDuration : 0f;

    public bool IsOnCooldown => (Time.time - lastHitTime) < hitCooldown;

    void HandleHit()
    {
        if (Input.GetMouseButtonDown(0) && !isHitting && !IsOnCooldown)
        {
            isHitting   = true;
            hitTimer    = hitDuration;
            lastHitTime = Time.time;

            if (hitAudioSource != null && !hitAudioSource.isPlaying)
                hitAudioSource.Play();
        }

        if (!isHitting) { hitBoostCurve = 0f; return; }

        hitTimer -= Time.deltaTime;

        if (hitTimer <= 0f)
        {
            isHitting    = false;
            hitTimer     = 0f;
            hitBoostCurve = 0f;
            return;
        }

        float progress = hitTimer / hitDuration;
        hitBoostCurve  = Mathf.SmoothStep(0f, 1f, progress);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isHitting) return;

        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        if (pushDir.magnitude < 0.1f) return;

        rb.AddForce(pushDir * hitForce * hitBoostCurve, ForceMode.Impulse);
    }
}
