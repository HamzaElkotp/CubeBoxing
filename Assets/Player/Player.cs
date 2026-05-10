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

        // ── Ground detection via sphere cast (works on any surface, not just y=0) ──
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset downward velocity when grounded so we don't accumulate infinite gravity
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // small negative keeps CheckSphere reliable
        }

        // ── Horizontal movement (unchanged input logic) ──
        Vector3 inputVect = gameInput.GetMovementVector().normalized;

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

        // ── Hit boost: tick the hit state, then scale move speed ──
        HandleHit();
        float currentSpeed = isHitting
            ? moveSpeed * Mathf.Lerp(1f, hitSpeedMultiplier, hitBoostCurve)
            : moveSpeed;

        Vector3 moveDir = (transform.right * inputVect.x + transform.forward * inputVect.z) * currentSpeed;

        // ── Move via CharacterController (collision-safe) ──
        Vector3 finalMovement = moveDir;
        finalMovement.y = verticalVelocity;
        controller.Move(finalMovement * Time.deltaTime);
    }


    [Header("Hit Settings")]
    [SerializeField] private float hitSpeedMultiplier = 2.5f;  // peak speed scale during hit
    [SerializeField] private float hitForce           = 18f;   // impulse sent to struck rigidbodies
    [SerializeField] private float hitDuration        = 0.5f;  // seconds the boost lasts
    [SerializeField] private float hitCooldown        = 3f;    // seconds before next hit is allowed
    [SerializeField] private AudioSource hitAudioSource;       // optional swipe/punch sound

    private bool  isHitting    = false;
    private float hitTimer     = 0f;   // counts DOWN from hitDuration → 0
    private float lastHitTime  = -999f;
    private float hitBoostCurve = 0f;  // 0–1 smooth value used to scale speed & force

    /// <summary>0–1 progress of the active hit (1 = peak, 0 = finished). Useful for UI.</summary>
    public float HitProgress => isHitting ? hitTimer / hitDuration : 0f;

    /// <summary>True while the cooldown has not yet expired.</summary>
    public bool IsOnCooldown => (Time.time - lastHitTime) < hitCooldown;

    void HandleHit()
    {
        // Trigger on left-click, only when not already hitting and cooldown expired
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

        // progress goes 1→0 as the hit expires.
        // SmoothStep converts that into a curve that peaks sharply then eases out —
        // giving the "punch" feel: instant burst, smooth fade.
        float progress = hitTimer / hitDuration;          // 1 at start, 0 at end
        hitBoostCurve  = Mathf.SmoothStep(0f, 1f, progress);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isHitting) return;

        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;

        // Only push horizontally, and only when moving toward the object
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        if (pushDir.magnitude < 0.1f) return;

        // Force is strongest at the moment of impact and fades with hitBoostCurve
        rb.AddForce(pushDir * hitForce * hitBoostCurve, ForceMode.Impulse);
    }
}
