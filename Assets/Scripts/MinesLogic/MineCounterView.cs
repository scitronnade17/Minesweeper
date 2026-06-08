using UnityEngine;
using TMPro;
using Zenject;

public class MineCounterView : MonoBehaviour
{
    private TMP_Text counterText;

    private IConfigDataService configDataService;

    private CellPresenter[,] cells;
    private int totalMines;

    [Inject]
    public void Construct(IConfigDataService _configDataService)
    {
        configDataService = _configDataService;
    }

    private void Awake()
    {
        counterText = GetComponent<TMP_Text>();
    }

    public void Bind(CellPresenter[,] _cells)
    {
        cells = _cells;
        
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        var validMines = configDataService.GetGameData().minesCount;
        totalMines = Mathf.Clamp(validMines, 1, width * height - 1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                cells[x, y].OnFlagChanged += Refresh;

        Refresh();
    }

    private void Refresh()
    {
        int flags = CountFlags();
        counterText.text = (totalMines - flags).ToString();
    }

    private int CountFlags()
    {
        int count = 0;
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (cells[x, y].IsFlag) count++;

        return count;
    }

    private void OnDestroy()
    {
        if (cells == null) return;

        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                cells[x, y].OnFlagChanged -= Refresh;
    }
}