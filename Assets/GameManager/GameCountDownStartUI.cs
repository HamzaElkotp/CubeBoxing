using TMPro;
using UnityEngine;

public class GameCountDownStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStatChanged;

        gameObject.SetActive(false);
    }

    private void GameManager_OnStatChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToSTartActive())
        {
            gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        countdownText.text = ((int)GameManager.Instance.GetCountDownToStartTimer()).ToString();
    }

}
