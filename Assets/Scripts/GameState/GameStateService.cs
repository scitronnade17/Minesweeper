using System;

public interface IGameStateService
{
    GameState Current { get; }

    event Action<GameState> OnStateChanged;

    void Set(GameState state);
    void Reset();
}

public class GameStateService : IGameStateService
{
    public GameState Current { get; private set; } = GameState.MainMenu;

    public event Action<GameState> OnStateChanged;

    public void Set(GameState state)
    {
        if (Current == state) return;

        Current = state;
        OnStateChanged?.Invoke(state);
    }

    public void Reset()
    {
        if (Current == GameState.MainMenu)
            return;

        Current = GameState.Playing;
        OnStateChanged?.Invoke(GameState.Playing);
    }
}