using UnityEngine;
using TMPro;
using Zenject;

public class TimerView : MonoBehaviour
{
    private TMP_Text timerText;

    private ITimerService timer;
    private IGameStateService gameState;

    [Inject]
    public void Construct(ITimerService _timer, IGameStateService _gameState)
    {
        timer = _timer;
        gameState = _gameState;
    }

    private void Start()
    {
        timerText = GetComponent<TMP_Text>();

        UpdateDisplay(0f);

        timer.OnTick += UpdateDisplay;
        gameState.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                timer.Pause();
                break;

            case GameState.Playing:
                if (timer.ElapsedSeconds > 0f)
                    timer.Resume();
                break;

            case GameState.Won:
            case GameState.Lost:
                timer.Stop();
                break;

            case GameState.MainMenu:
                timer.Reset();
                UpdateDisplay(0f);
                break;
        }
    }

    private void UpdateDisplay(float seconds)
    {
        int mins = (int)seconds / 60;
        int secs = (int)seconds % 60;
        timerText.text = $"{mins:00}:{secs:00}";
    }

    private void OnDestroy()
    {
        timer.OnTick -= UpdateDisplay;
        gameState.OnStateChanged -= OnStateChanged;
    }
}