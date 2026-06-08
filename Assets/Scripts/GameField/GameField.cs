using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameField : MonoBehaviour
{
    [SerializeField] private MineCounterView mineCounterView;

    private IGridFactory gridFactory;
    private ITimerService timer;

    public CellPresenter[,] Cells { get; private set; }
    public bool IsBuilt { get; private set; }

    [Inject]
    public void Construct(
        IGridFactory _gridFactory,
        IConfigDataService _configDataService,
        ITimerService _timer)
    {
        gridFactory = _gridFactory;
        timer = _timer;
    }

    private void Awake()
    {
        timer.Reset();
    }

    public void Build()
    {
        IsBuilt = true;
        Cells = gridFactory.Build(transform);
        mineCounterView.Bind(Cells);
    }

    public void Reset()
    {
        if (Cells != null)
        {
            Cells = null;
        }

        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        foreach (var child in children)
            Destroy(child);

        IsBuilt = false;
        timer.Reset();
    }
}