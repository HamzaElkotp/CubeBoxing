using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 GetMovementVector()
    {
        Vector3 inputVect = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputVect.z = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputVect.z = -1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputVect.x = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputVect.x = 1;

        return inputVect;
    }

    public bool IsJumpPressed()
    {
        // GetKeyDown returns true only on the frame the button is first pressed
        return Input.GetKeyDown(KeyCode.Space);
    }
}
