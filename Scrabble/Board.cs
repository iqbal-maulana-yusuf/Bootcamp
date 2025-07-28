public interface IBoard
{
    Square[,] GetGrid();
    Square GetSquare(int x, int y);
    void InitializeBonusSquares();
}

public class Board : IBoard
{
    private const int BOARD_SIZE = 15;
    private const int CENTER_POSITION = 7;
    private Square[,] _grid;

    public Board()
    {
        _grid = new Square[BOARD_SIZE, BOARD_SIZE];
        InitializeBonusSquares();
    }

    public Square[,] GetGrid() => _grid;

    public Square GetSquare(int x, int y)
    {
        if (x < 0 || x >= BOARD_SIZE || y < 0 || y >= BOARD_SIZE)
            throw new ArgumentOutOfRangeException();
        return _grid[x, y];
    }

    public void InitializeBonusSquares()
    {
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                _grid[x, y] = new Square(x, y, BonusSquareType.Normal);
            }
        }

        _grid[CENTER_POSITION, CENTER_POSITION] = new Square(CENTER_POSITION, CENTER_POSITION, BonusSquareType.DoubleWord);
    }
}
