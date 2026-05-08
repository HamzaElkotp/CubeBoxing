using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float rotationSpeed = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVect = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            Debug.Log("W");
            inputVect.z = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            Debug.Log("S");
            inputVect.z = -1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            Debug.Log("A");
            inputVect.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            Debug.Log("A");
            inputVect.x = 1;
        }
        if (Input.GetKey(KeyCode.Space)){
            inputVect.y = 1;
        }

        inputVect = inputVect.normalized;
        transform.position += inputVect * Time.deltaTime * moveSpeed;

        transform.forward = Vector3.Slerp(transform.forward, inputVect, Time.deltaTime * rotationSpeed);

        Debug.Log(inputVect);
    }
}
