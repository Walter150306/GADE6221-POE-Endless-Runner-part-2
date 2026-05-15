using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseContainer;

    [Header("Optional Blockers")]
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;

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

        if (levelCompletePanel != null && levelCompletePanel.activeSelf)
        {
            return;
        }

        bool escapePressed = false;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            escapePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePressed = true;
        }

        if (escapePressed)
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

        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        if (pauseContainer != null)
        {
            pauseContainer.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;

        Debug.Log("Game resumed.");
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