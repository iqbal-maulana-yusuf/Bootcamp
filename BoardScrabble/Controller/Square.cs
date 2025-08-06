using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
    public class Square
    {
        private int _x;
        private int _y;
        private BonusSquareType _bonusType;
        private Tile? _currentTile;

        public Square(int x, int y, BonusSquareType bonusType)
        {
            _x = x;
            _y = y;
            _bonusType = bonusType;
            _currentTile = null;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public BonusSquareType GetBonusType()
        {
            return _bonusType;
        }

        public Tile? GetTile()
        {
            return _currentTile;
        }

        public void SetTile(Tile tile)
        {
            _currentTile = tile;
        }

        public void SetX(int x)
        {
            _x = x;
        }

        public void SetY(int y)
        {
            _y = y;
        }

        public void SetBonusType(BonusSquareType bonusType)
        {
            _bonusType = bonusType;
        }
    }
}
