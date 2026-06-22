using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject metricsPanel;

    public void PlayGame()
    {
        RunProgressManager.Instance.StartNewRun();
        GameEventManager.Instance.ResetRunMetrics();

        SceneManager.LoadScene("Level 1");
    }

    public void ShowMetrics()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        if (metricsPanel != null)
        {
            metricsPanel.SetActive(true);
        }

        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.RefreshLeaderboardText();
        }
    }

    public void HideMetrics()
    {
        if (metricsPanel != null)
        {
            metricsPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}