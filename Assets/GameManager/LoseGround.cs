using UnityEngine;

/// <summary>
/// Attach this to any GameObject that has a Collider set to "Is Trigger".
/// When the Player walks or falls into it, the game is set to GameOver.
/// </summary>
public class LoseGround : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check by tag so we don't fire on random objects
        if (other.CompareTag("Player"))
        {
            Debug.Log("Lose");
            GameManager.Instance.SetGameOver();
        }
    }
}
