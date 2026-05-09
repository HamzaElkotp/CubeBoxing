using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private AudioSource moveAudioSource;
    [SerializeField] private AudioSource jumpAudioSource;

    private float verticalVelocity;
    private bool isGrounded;
    private bool jumped = false;

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

        bool isMoving = (inputVect.x + inputVect.z) != 0;

        if (isMoving)
        {
            Debug.Log("movings");
            if (!moveAudioSource.isPlaying)
            {
                moveAudioSource.Play();
            }
        }
        else
        {
            Debug.Log("stoped");
            if (moveAudioSource.isPlaying)
            {
                moveAudioSource.Stop();
            }
        }

        isGrounded = transform.position.y <= 0f;

        if (isGrounded && verticalVelocity <= 0 && jumped)
        {
            if (!jumpAudioSource.isPlaying)
            {
                jumpAudioSource.Play();
            }
            verticalVelocity = 0f;
            jumped = false;
        }

        if (isGrounded && gameInput.IsJumpPressed())
        {
            jumped = true;
            verticalVelocity = jumpForce;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDir;
        finalMovement.y = verticalVelocity;

        transform.position += finalMovement * Time.deltaTime;


        // Keep player from falling through the floor
        if (transform.position.y < 0) transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
