using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumbHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    [Header("Push Settings")]
    public float pushDuration;
    public float pushForce;
    public float pushSpeedMultiplier;
    public float pushCooldown = 5f;
    private float lastPushTime;

    private bool isPushing = false;
    private float pushTimer = 0f;
    private float originalSpeed;

    void StartPush()
    {
        isPushing = true;
        pushTimer = pushDuration;
        speed = originalSpeed * pushSpeedMultiplier;
    }

    void HandlePush()
    {
        if (!isPushing) return;

        pushTimer -= Time.deltaTime;

        if (pushTimer <= 0f)
        {
            isPushing = false;
            speed = originalSpeed;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic) return;

        // Only push when we are actually moving forward
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        //Vector3 pushDir = transform.forward;

        if (pushDir.magnitude < 0.1f) return;

        float forceMultiplier = 1f;

        if (isPushing)
        {
            // Fade force over time
            forceMultiplier = pushTimer / pushDuration;
        }

        float finalForce = pushForce * forceMultiplier;

        rb.AddForce(pushDir * finalForce, ForceMode.Impulse);
    }

    void Start() {
        controller = GetComponent<CharacterController>();
        originalSpeed = speed;
    }

    void Update(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;


        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumbHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        if (lastPosition != gameObject.transform.position && isGrounded){
            isMoving = true;

        } else {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;

        if (Input.GetMouseButtonDown(0) && Time.time >= lastPushTime + pushCooldown)
        {
            StartPush();
            lastPushTime = Time.time;
        }

        HandlePush();
    }
}
