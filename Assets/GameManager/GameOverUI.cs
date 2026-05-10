using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Over overlay UI.
/// Hooks into GameManager.OnStateChanged exactly like GameCountDownStartUI.
/// Assign this script to the root of your Game Over canvas panel.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        gameObject.SetActive(false);         // hidden until game over
    }

    private void OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;   // release cursor so player can click buttons
            Cursor.visible   = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // ── Button callbacks (wire these in the Inspector) ──────────────

    /// <summary>Restart: reload the current scene.</summary>
    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Quit: exit to the main menu (scene index 0) or quit application.</summary>
    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
