using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private IGameStateService gameState;

    [Inject]
    public void Construct(IGameStateService _gameState)
    {
        gameState = _gameState;
    }

    private void Start()
    {
        gameState.OnStateChanged += OnStateChanged;
        startButton.onClick.AddListener(StartGame);

        gameObject.SetActive(gameState.Current == GameState.MainMenu);
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
            gameObject.SetActive(true);
    }

    private void StartGame()
    {
        gameObject.SetActive(false);
        gameState.Set(GameState.Playing);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
        gameState.OnStateChanged -= OnStateChanged;
    }
}