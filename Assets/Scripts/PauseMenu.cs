using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("Scene References")]
    public string mainMenuScene;
    public string gameScene;

    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction pauseAction;

    [Header("Events")]
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip gameOverSound;
    [Range(0f, 1f)] public float gameOverVolume;

    private bool paused = false;

    private void Awake()
    {
        // Reset time scale when scene loads
        Time.timeScale = 1f;
        paused = false;
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.FindActionMap("Player").Enable();
            pauseAction = inputActions.FindAction("Pause");
            pauseAction.performed += OnPausePerformed;
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.FindActionMap("Player").Disable();
            Cleanup();
        }
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        PlayGameOverSound();

        // Pause the game
        Time.timeScale = 0f;

        // Disable pause input when game over
        if (inputActions != null)
        {
            inputActions.FindActionMap("Player").Disable();
        }

        OnGamePaused?.Invoke();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        OnGamePaused?.Invoke();
    }

    private void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        OnGameResumed?.Invoke();
    }

    public void Restart()
    {
        Cleanup();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(gameScene);
    }

    public void Quit()
    {
        Cleanup();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(mainMenuScene);
    }

    private void Cleanup()
    {
        // Clean up before scene change
        pauseAction.performed -= OnPausePerformed;
    }

    private void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound, gameOverVolume);
        }
    }
}