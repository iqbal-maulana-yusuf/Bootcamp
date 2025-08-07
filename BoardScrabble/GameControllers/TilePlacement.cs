using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
    public struct TilePlacement
    {
        private Tile _tile;
        private int _x;
        private int _y;

        public TilePlacement(Tile tile, int x, int y)
        {
            _tile = tile;
            _x = x;
            _y = y;
        }

        public Tile GetTile()
        {
            return _tile;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public void SetTile(Tile tile)
        {
            _tile = tile;
        }

        public void SetX(int x)
        {
            _x = x;
        }

        public void SetY(int y)
        {
            _y = y;
        }
    }
}
