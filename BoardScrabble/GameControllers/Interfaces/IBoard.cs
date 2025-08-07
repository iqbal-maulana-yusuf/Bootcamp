using BoardScrabble.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.GameControllers.Interfaces
{
    public interface IBoard
    {
        Square[,] GetGrid();
        Square GetSquare(int x, int y);
        void InitializeBonusSquares();
        void SetGrid(Square[,] grid);
        void SetSquare(int x, int y, Square square);

        int GetBoardSize();
    }
}
