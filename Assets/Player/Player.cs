using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private GameInput gameInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVect = gameInput.GetMovementVectorNormalize();

        inputVect = inputVect.normalized;
        transform.position += inputVect * Time.deltaTime * moveSpeed;

        //transform.forward = Vector3.Slerp(transform.forward, inputVect, Time.deltaTime * rotationSpeed);
    }
}
