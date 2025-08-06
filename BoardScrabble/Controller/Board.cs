using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
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
            // Triple Word (red)
            SetBonusSquare(0, 0, BonusSquareType.TripleWord);
            SetBonusSquare(0, 7, BonusSquareType.TripleWord);
            SetBonusSquare(0, 14, BonusSquareType.TripleWord);
            SetBonusSquare(7, 0, BonusSquareType.TripleWord);
            SetBonusSquare(7, 14, BonusSquareType.TripleWord);
            SetBonusSquare(14, 0, BonusSquareType.TripleWord);
            SetBonusSquare(14, 7, BonusSquareType.TripleWord);
            SetBonusSquare(14, 14, BonusSquareType.TripleWord);

            // Double Word (pink)
            SetBonusSquare(1, 1, BonusSquareType.DoubleWord);
            SetBonusSquare(2, 2, BonusSquareType.DoubleWord);
            SetBonusSquare(3, 3, BonusSquareType.DoubleWord);
            SetBonusSquare(4, 4, BonusSquareType.DoubleWord);
            SetBonusSquare(10, 10, BonusSquareType.DoubleWord);
            SetBonusSquare(11, 11, BonusSquareType.DoubleWord);
            SetBonusSquare(12, 12, BonusSquareType.DoubleWord);
            SetBonusSquare(13, 13, BonusSquareType.DoubleWord);
            SetBonusSquare(7, 7, BonusSquareType.DoubleWord); // Center star
            SetBonusSquare(10, 4, BonusSquareType.DoubleWord);
            SetBonusSquare(11, 3, BonusSquareType.DoubleWord);
            SetBonusSquare(12, 2, BonusSquareType.DoubleWord);
            SetBonusSquare(13, 1, BonusSquareType.DoubleWord);
            SetBonusSquare(4, 10, BonusSquareType.DoubleWord);
            SetBonusSquare(3, 11, BonusSquareType.DoubleWord);
            SetBonusSquare(2, 12, BonusSquareType.DoubleWord);
            SetBonusSquare(1, 13, BonusSquareType.DoubleWord);


            // Triple Letter (blue)
            SetBonusSquare(1, 5, BonusSquareType.TripleLetter);
            SetBonusSquare(1, 9, BonusSquareType.TripleLetter);
            SetBonusSquare(5, 1, BonusSquareType.TripleLetter);
            SetBonusSquare(5, 5, BonusSquareType.TripleLetter);
            SetBonusSquare(5, 9, BonusSquareType.TripleLetter);
            SetBonusSquare(5, 13, BonusSquareType.TripleLetter);
            SetBonusSquare(9, 1, BonusSquareType.TripleLetter);
            SetBonusSquare(9, 5, BonusSquareType.TripleLetter);
            SetBonusSquare(9, 9, BonusSquareType.TripleLetter);
            SetBonusSquare(9, 13, BonusSquareType.TripleLetter);
            SetBonusSquare(13, 5, BonusSquareType.TripleLetter);
            SetBonusSquare(13, 9, BonusSquareType.TripleLetter);

            // Double Letter (light blue)
            SetBonusSquare(0, 3, BonusSquareType.DoubleLetter);
            SetBonusSquare(0, 11, BonusSquareType.DoubleLetter);
            SetBonusSquare(2, 6, BonusSquareType.DoubleLetter);
            SetBonusSquare(2, 8, BonusSquareType.DoubleLetter);
            SetBonusSquare(3, 0, BonusSquareType.DoubleLetter);
            SetBonusSquare(3, 7, BonusSquareType.DoubleLetter);
            SetBonusSquare(3, 14, BonusSquareType.DoubleLetter);
            SetBonusSquare(6, 2, BonusSquareType.DoubleLetter);
            SetBonusSquare(6, 6, BonusSquareType.DoubleLetter);
            SetBonusSquare(6, 8, BonusSquareType.DoubleLetter);
            SetBonusSquare(6, 12, BonusSquareType.DoubleLetter);
            SetBonusSquare(7, 3, BonusSquareType.DoubleLetter);
            SetBonusSquare(7, 11, BonusSquareType.DoubleLetter);
            SetBonusSquare(8, 2, BonusSquareType.DoubleLetter);
            SetBonusSquare(8, 6, BonusSquareType.DoubleLetter);
            SetBonusSquare(8, 8, BonusSquareType.DoubleLetter);
            SetBonusSquare(8, 12, BonusSquareType.DoubleLetter);
            SetBonusSquare(11, 0, BonusSquareType.DoubleLetter);
            SetBonusSquare(11, 7, BonusSquareType.DoubleLetter);
            SetBonusSquare(11, 14, BonusSquareType.DoubleLetter);
            SetBonusSquare(12, 6, BonusSquareType.DoubleLetter);
            SetBonusSquare(12, 8, BonusSquareType.DoubleLetter);
            SetBonusSquare(14, 3, BonusSquareType.DoubleLetter);
            SetBonusSquare(14, 11, BonusSquareType.DoubleLetter);
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


        public void Display()
        {
            Console.Write("   ");
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                Console.Write($"{x:D2} ");
            }
            Console.WriteLine();

            for (int y = 0; y < BOARD_SIZE; y++)
            {
                Console.Write($"{y:D2} ");
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    var square = _grid[x, y];
                    var tile = square.GetTile();

                    if (tile != null)
                    {
                        Console.Write($" {tile.GetLetter()} ");
                    }
                    else
                    {
                        // Special handling for center square
                        if (x == CENTER_POSITION && y == CENTER_POSITION && square.GetBonusType() == BonusSquareType.DoubleWord)
                        {
                            Console.Write(" ★ "); // Star symbol for center
                        }
                        else
                        {
                            Console.Write($" {GetBonusSymbol(square.GetBonusType())} ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private char GetBonusSymbol(BonusSquareType type)
        {
            return type switch
            {
                BonusSquareType.TripleWord => '3',
                BonusSquareType.DoubleWord => '2',
                BonusSquareType.TripleLetter => '#',
                BonusSquareType.DoubleLetter => '+',
                BonusSquareType.Normal => '.',
                _ => '?'
            };
        }




    }


}
