using System.Collections.Generic;

public interface IEmptyCellOpener
{
    void Expand(CellPresenter[,] cells, int startX, int startY);
}

public class EmptyCellOpener : IEmptyCellOpener
{
    public void Expand(CellPresenter[,] cells, int startX, int startY)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int x, int y)>();

        Enqueue(startX, startY);

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            CellPresenter current = cells[cx, cy];

            current.Reveal();

            if (!current.IsEmpty || current.IsMine) continue;

            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = cx + dx;
                    int ny = cy + dy;

                    if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;

                    CellPresenter neighbour = cells[nx, ny];

                    if (neighbour.IsMine || neighbour.IsReveal || neighbour.IsFlag) continue;

                    Enqueue(nx, ny);
                }
        }

        void Enqueue(int x, int y)
        {
            if (visited.Add((x, y)))
                queue.Enqueue((x, y));
        }
    }
}