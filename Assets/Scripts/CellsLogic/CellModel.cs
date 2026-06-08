using System;
using UnityEngine;

public class CellModel
{
    public Vector2Int Position { get; }

    public bool IsMine { get; private set; }
    public bool IsReveal { get; private set; }
    public bool IsFlag { get; private set; }
    public bool IsEmpty { get; private set; }
    public int NeighbourMineCount { get; private set; }

    public event Action<CellModel> OnStateChanged;

    public CellModel(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public void SetMine()
    {
        IsMine = true;
    }

    public void ClearMine()
    {
        IsMine = false;
    }

    public void SetNumber(int count)
    {
        NeighbourMineCount = count;
        IsEmpty = !IsMine && count == 0;
    }

    public void Reveal()
    {
        if (IsReveal) return;
        IsReveal = true;
        OnStateChanged?.Invoke(this);
    }

    public void ToggleFlag()
    {
        if (IsReveal) return;
        IsFlag = !IsFlag;
        OnStateChanged?.Invoke(this);
    }
}