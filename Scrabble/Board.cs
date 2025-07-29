public interface IBoard
    {
        Square[,] GetGrid();
        Square GetSquare(int x, int y);
        void InitializeBonusSquares();
        void SetGrid(Square[,] grid);
        void SetSquare(int x, int y, Square square);
    }

public class Board : IBoard
    {
        private Square[,] _grid;
        private const int BOARD_SIZE = 15;
        private const int CENTER_POSITION = 7;

        public Board()
        {
            _grid = new Square[BOARD_SIZE, BOARD_SIZE];
            // Inisialisasi semua square sebagai Normal terlebih dahulu
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    _grid[i, j] = new Square(i, j, BonusSquareType.Normal);
                }
            }
            InitializeBonusSquares();
        }

        public Square[,] GetGrid()
        {
            return _grid;
        }

        public Square GetSquare(int x, int y)
        {
            if (x < 0 || x >= BOARD_SIZE || y < 0 || y >= BOARD_SIZE)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of board bounds.");
            }
            return _grid[x, y];
        }

        public void InitializeBonusSquares()
        {
            SetBonusSquare(0, 0, BonusSquareType.TripleWord);
            SetBonusSquare(0, 7, BonusSquareType.TripleWord);
            SetBonusSquare(0, 14, BonusSquareType.TripleWord);
            SetBonusSquare(7, 0, BonusSquareType.TripleWord);
            SetBonusSquare(7, 14, BonusSquareType.TripleWord);
            SetBonusSquare(14, 0, BonusSquareType.TripleWord);
            SetBonusSquare(14, 7, BonusSquareType.TripleWord);
            SetBonusSquare(14, 14, BonusSquareType.TripleWord);

            SetBonusSquare(1, 1, BonusSquareType.DoubleWord);
            SetBonusSquare(2, 2, BonusSquareType.DoubleWord);
            SetBonusSquare(3, 3, BonusSquareType.DoubleWord);
            SetBonusSquare(4, 4, BonusSquareType.DoubleWord);
            SetBonusSquare(7, 7, BonusSquareType.DoubleWord); // Central star

     
            SetBonusSquare(0, 3, BonusSquareType.DoubleLetter);
            SetBonusSquare(0, 11, BonusSquareType.DoubleLetter);
   
            SetBonusSquare(1, 5, BonusSquareType.TripleLetter);
            SetBonusSquare(1, 9, BonusSquareType.TripleLetter);
        }

        private void SetBonusSquare(int x, int y, BonusSquareType type)
        {
            _grid[x, y].SetBonusType(type);
        }

        public void SetGrid(Square[,] grid)
        {
            _grid = grid;
        }

        public void SetSquare(int x, int y, Square square)
        {
            if (x < 0 || x >= BOARD_SIZE || y < 0 || y >= BOARD_SIZE)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of board bounds.");
            }
            _grid[x, y] = square;
        }
    }

