public interface INeighbourMineCounter
{
    void Apply(CellPresenter[,] cells);
}

public class NeighbourMineCounter : INeighbourMineCounter
{
    public void Apply(CellPresenter[,] cells)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (cells[x, y].IsMine) continue;

                int count = CountNeighbourMines(cells, x, y, width, height);
                cells[x, y].SetNumber(count);
            }
    }

    private int CountNeighbourMines(CellPresenter[,] cells, int cx, int cy, int width, int height)
    {
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = cx + dx;
                int ny = cy + dy;

                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;

                if (cells[nx, ny].IsMine) count++;
            }

        return count;
    }
}