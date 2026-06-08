using UnityEngine;
using Zenject;

public interface IGridFactory
{
    CellPresenter[,] Build(Transform parent);
}

public class GridFactory : IGridFactory
{
    private readonly IConfigDataService configDataService;
    private readonly IMinePlacer minePlacer;
    private readonly INeighbourMineCounter mineCounter;
    private readonly DiContainer container;

    public GridFactory(
        IConfigDataService _configDataService,
        IMinePlacer _minePlacer,
        INeighbourMineCounter _mineCounter,
        DiContainer _container)
    {
        configDataService = _configDataService;
        minePlacer = _minePlacer;
        mineCounter = _mineCounter;
        container = _container;
    }

    public CellPresenter[,] Build(Transform parent)
    {
        GameData data = configDataService.GetGameData();
        int width = data.fieldWidth;
        int height = data.fieldHeight;

        var cells = new CellPresenter[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var model = new CellModel(x, y);

                GameObject go = container.InstantiatePrefab(
                    data.cellPrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    parent);

                var view = go.GetComponent<CellView>();
                view.Init(data.emptySprite, data.flagSprite, data.mineSprite);

                cells[x, y] = new CellPresenter(model, view);
            }
        }

        minePlacer.Place(cells, data.minesCount);
        mineCounter.Apply(cells);

        return cells;
    }
}