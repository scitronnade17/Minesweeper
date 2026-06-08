using UnityEngine;
using Zenject;

public class GameFieldController : MonoBehaviour
{
    private GameField field;
    private CellInputHandler inputHandler;
    private IGameStateService gameState;
    private IConfigDataService configDataService;
    private GameFieldLayout layout;

    [Inject]
    public void Construct(
        CellInputHandler _inputHandler,
        IGameStateService _gameState,
        IConfigDataService _configDataService)
    {
        inputHandler = _inputHandler;
        gameState = _gameState;
        configDataService = _configDataService;
    }

    private void Awake()
    {
        gameState.Reset();
        field = GetComponent<GameField>();
    }

    private void Start()
    {
        configDataService.Load();
        layout = field.GetComponent<GameFieldLayout>();
        layout.Apply();

        gameState.OnStateChanged += OnStateChanged;

        if (gameState.Current == GameState.Playing && !field.IsBuilt)
            BuildGrid();
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing && !field.IsBuilt)
            BuildGrid();
        if (state == GameState.MainMenu)
            ResetGrid();
    }

    private void BuildGrid()
    {
        field.Build();
        inputHandler.ResetFirstClick();
        SubscribeCells();
    }

    private void ResetGrid()
    {
        UnsubscribeCells();
        field.Reset();
    }

    private void SubscribeCells()
    {
        var cells = field.Cells;
        int w = cells.GetLength(0), h = cells.GetLength(1);
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                var cell = cells[x, y];
                cell.OnLeftClick += c => inputHandler.HandleLeftClick(c, field.Cells);
                cell.OnRightClick += c => inputHandler.HandleRightClick(c, field.Cells);
            }
    }

    private void UnsubscribeCells()
    {
        if (field.Cells == null) return;
        var cells = field.Cells;
        int w = cells.GetLength(0), h = cells.GetLength(1);
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                cells[x, y].OnLeftClick -= c => inputHandler.HandleLeftClick(c, field.Cells);
                cells[x, y].OnRightClick -= c => inputHandler.HandleRightClick(c, field.Cells);
            }
    }

    private void OnDestroy()
    {
        gameState.OnStateChanged -= OnStateChanged;
        UnsubscribeCells();
    }
}