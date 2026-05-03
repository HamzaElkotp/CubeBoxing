using UnityEngine;

public class PlayerPushController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Movement")]
    public float speed = 12f;
    public float pushSpeedMultiplier = 10f;

    [Header("Push Settings")]
    public float pushDorce = 12f;
    public float pushDuration = 1f;

    private float pushTimer = 0f;
    private bool isPushing = false;

    private Vector3 moveDirection;

    void StartPush() { 
        isPushing = true;
        pushTimer = pushDuration;
    }
    void HandleMovement() {
        float currentSpeed = speed;

    }

    void FixedUpdate() {
        HandleMovement();
        //HandlePushDecay();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection = new Vector3(h, 0, v).normalized;

        if (Input.GetMouseButtonDown(0)) {
            StartPush();
        }
    }
}
