using System.Collections.Generic;
using UnityEngine;

public interface IMinePlacer
{
    void Place(CellPresenter[,] cells, int count);
    void EnsureSafeCell(CellPresenter[,] cells, Vector2Int safePosition);
}

public class MinePlacer : IMinePlacer
{
    public void Place(CellPresenter[,] cells, int count)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);
        int total = width * height;

        int mineCount = Mathf.Clamp(count, 1, total - 1);

        var indices = new List<int>(total);
        for (int i = 0; i < total; i++) indices.Add(i);

        for (int i = total - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        for (int i = 0; i < mineCount; i++)
        {
            int x = indices[i] % width;
            int y = indices[i] / width;
            cells[x, y].SetMine();
        }
    }

    public void EnsureSafeCell(CellPresenter[,] cells, Vector2Int safePosition)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        CellPresenter target = cells[safePosition.x, safePosition.y];
        if (!target.IsMine) return;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CellPresenter cp = cells[x, y];
                if (cp.IsMine) continue;
                if (cp.Position == safePosition) continue;

                cp.SetMine();
                target.ClearMine();
                return;
            }
        }
    }
}