using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 GetMovementVectorNormalize()
    {
        Vector3 inputVect = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("W");
            inputVect.z = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("S");
            inputVect.z = -1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("A");
            inputVect.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("A");
            inputVect.x = 1;
        }
        if (Input.GetKey(KeyCode.Space) && transform.position.y == 0)
        {
            inputVect.y = 1;
        }

        return inputVect;
    }
}
