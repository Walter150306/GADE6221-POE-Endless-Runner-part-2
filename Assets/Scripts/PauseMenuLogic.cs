using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseContainer;

    [Header("Optional")]
    public GameObject gameOverPanel;

    private bool isPaused = false;

    void Start()
    {
        if (pauseContainer != null)
        {
            pauseContainer.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseContainer != null)
        {
            pauseContainer.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseContainer != null)
        {
            pauseContainer.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}