using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float jumpForce = 120f;
    [SerializeField] private float gravity = -20.81f;
    [SerializeField] private GameInput gameInput;

    private float verticalVelocity;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVect = gameInput.GetMovementVector();

        inputVect = inputVect.normalized;
        Vector3 moveDir = inputVect * moveSpeed;

        isGrounded = transform.position.y <= 0.1f;

        if (isGrounded && verticalVelocity < 0)
        {
            // Reset velocity when hitting the floor
            verticalVelocity = -2f;
        }

        if (isGrounded && gameInput.IsJumpPressed())
        {
            verticalVelocity = jumpForce;
        }

        //verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDir;
        finalMovement.y = verticalVelocity;

        transform.position += finalMovement * Time.deltaTime;


        // Keep player from falling through the floor
        if (transform.position.y < 0) transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
