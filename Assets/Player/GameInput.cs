using UnityEngine;

public class GameInput : MonoBehaviour
{
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
        return Input.GetKeyDown(KeyCode.Space);
    }
}
