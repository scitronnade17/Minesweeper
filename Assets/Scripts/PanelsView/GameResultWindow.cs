using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Zenject;

public class GameResultWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;

    private string winTitle = "Win!";
    private string loseTitle = "Lose!";

    private IGameStateService gameState;

    [Inject]
    public void Construct(IGameStateService _gameState)
    {
        gameState = _gameState;
    }

    private void Start()
    {
        gameObject.SetActive(false);

        gameState.OnStateChanged += OnStateChanged;
        restartButton.onClick.AddListener(Restart);
        backButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            gameObject.SetActive(false);
            return;
        }
        if (state != GameState.Won && state != GameState.Lost) return;

        bool isWin = state == GameState.Won;
        titleText.text = isWin ? winTitle : loseTitle;
        gameObject.SetActive(true);
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
        restartButton.onClick.RemoveListener(Restart);
        backButton.onClick.RemoveListener(GoToMainMenu);
    }
}