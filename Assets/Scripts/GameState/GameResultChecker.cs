public interface IGameResultChecker
{
    GameState Check(CellPresenter[,] cells);
}

public class GameResultChecker : IGameResultChecker
{
    public GameState Check(CellPresenter[,] cells)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        bool hasWrongFlag = false;
        bool allNonRevealed = true;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CellPresenter cell = cells[x, y];

                if (cell.IsMine && cell.IsReveal)
                    return GameState.Lost;

                if (cell.IsFlag && !cell.IsMine)
                    hasWrongFlag = true;

                if (!cell.IsMine && !cell.IsReveal)
                    allNonRevealed = false;
            }
        }

        if (allNonRevealed && !hasWrongFlag)
            return GameState.Won;

        return GameState.Playing;
    }
}