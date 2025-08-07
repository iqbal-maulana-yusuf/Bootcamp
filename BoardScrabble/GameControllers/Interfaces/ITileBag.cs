using BoardScrabble.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.GameControllers.Interfaces
{
    public interface ITileBag
    {
        List<Tile> GetTilesList();
        void InitializeStandardTiles();
        void SetTilesList(List<Tile> tiles);

        Dictionary<string, int> GetTileCount();
    }
}
