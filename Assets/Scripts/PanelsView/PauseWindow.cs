using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;

    private IGameStateService gameState;

    [Inject]
    public void Construct(IGameStateService _gameState)
    {
        gameState = _gameState;
    }

    private void Start()
    {
        gameObject.SetActive(false);

        pauseButton.onClick.AddListener(OpenPause);
        resumeButton.onClick.AddListener(ClosePause);
        restartButton.onClick.AddListener(Restart);
        backButton.onClick.AddListener(GoToMainMenu);

        gameState.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Won || state == GameState.Lost || state == GameState.MainMenu)
            gameObject.SetActive(false);
    }

    private void OpenPause()
    {
        if (gameState.Current != GameState.Playing) return;

        gameState.Set(GameState.Paused);
        gameObject.SetActive(true);
    }

    private void ClosePause()
    {
        gameObject.SetActive(false);
        gameState.Set(GameState.Playing);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        gameState.Set(GameState.MainMenu);
    }

    private void OnDestroy()
    {
        gameState.OnStateChanged -= OnStateChanged;
        pauseButton.onClick.RemoveListener(OpenPause);
        resumeButton.onClick.RemoveListener(ClosePause);
        restartButton.onClick.RemoveListener(Restart);
        backButton.onClick.RemoveListener(GoToMainMenu);
    }
}