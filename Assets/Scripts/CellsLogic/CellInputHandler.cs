using Zenject;

public class CellInputHandler
{
    private readonly IMinePlacer minePlacer;
    private readonly INeighbourMineCounter mineCounter;
    private readonly IEmptyCellOpener opener;
    private readonly IGameResultChecker resultChecker;
    private readonly IGameStateService gameState;
    private readonly ITimerService timer;

    private bool isFirstClick = true;

    [Inject]
    public CellInputHandler(
        IMinePlacer _minePlacer,
        INeighbourMineCounter _mineCounter,
        IEmptyCellOpener _opener,
        IGameResultChecker _resultChecker,
        IGameStateService _gameState,
        ITimerService _timer)
    {
        minePlacer = _minePlacer;
        mineCounter = _mineCounter;
        opener = _opener;
        resultChecker = _resultChecker;
        gameState = _gameState;
        timer = _timer;
    }

    public void ResetFirstClick() => isFirstClick = true;

    public void HandleLeftClick(CellPresenter cell, CellPresenter[,] cells)
    {
        if (gameState.Current != GameState.Playing) return;
        if (cell.IsFlag) return;

        if (isFirstClick)
        {
            isFirstClick = false;
            minePlacer.EnsureSafeCell(cells, cell.Position);
            mineCounter.Apply(cells);
            timer.StartTimer();
        }

        if (cell.IsEmpty)
            opener.Expand(cells, cell.Position.x, cell.Position.y);
        else
            cell.Reveal();

        CheckResult(cells);
    }

    public void HandleRightClick(CellPresenter cell, CellPresenter[,] cells)
    {
        if (gameState.Current != GameState.Playing) return;

        cell.ToggleFlag();
        CheckResult(cells);
    }

    private void CheckResult(CellPresenter[,] cells)
    {
        GameState result = resultChecker.Check(cells);
        if (result != GameState.Playing)
            gameState.Set(result);
    }
}