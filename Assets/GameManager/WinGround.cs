using UnityEngine;

public class WinGround : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("win");
            GameManager.Instance.SetGameWin();
        }
    }
}
