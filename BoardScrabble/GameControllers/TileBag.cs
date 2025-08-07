using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
    using BoardScrabble.GameControllers.Interfaces;
    using System.Collections.Generic;

    public class TileBag : ITileBag
    {

        private List<Tile> _tiles;
        private Random _random;
        private Dictionary<string, int> _tileCount = new();

        public TileBag()
        {
            _tiles = new List<Tile>();
            _random = new Random();
            InitializeStandardTiles();
        }

        public List<Tile> GetTilesList()
        {
            return _tiles;
        }

        public void InitializeStandardTiles()
        {
            _tiles.Clear(); // Bersihkan jika sudah ada tile sebelumnya

            //Distribusi standar huruf dan poin di Scrabble(berdasarkan versi AS)
            AddTiles('A', 1, 9);
            AddTiles('B', 3, 2); AddTiles('C', 3, 2);
            AddTiles('D', 2, 4); AddTiles('E', 1, 12); AddTiles('F', 4, 2);
            AddTiles('G', 2, 3); AddTiles('H', 4, 2); AddTiles('I', 1, 9);
            AddTiles('J', 8, 1); AddTiles('K', 5, 1); AddTiles('L', 1, 4);
            AddTiles('M', 3, 2);
            AddTiles('N', 1, 6); AddTiles('O', 1, 8);
            AddTiles('P', 3, 2); AddTiles('Q', 10, 1); AddTiles('R', 1, 6);
            AddTiles('S', 1, 4); AddTiles('T', 1, 6); AddTiles('U', 1, 4);
            AddTiles('V', 4, 2); AddTiles('W', 4, 2); AddTiles('X', 8, 1);
            AddTiles('Y', 4, 2); AddTiles('Z', 10, 1);
            AddTiles(' ', 0, 2); // Blank tiles

            ShuffleTiles();
        }

        private void AddTiles(char letter, int points, int count)
        {
            _tileCount[letter.ToString()] = count;
            for (int i = 0; i < count; i++)
            {
                _tiles.Add(new Tile(letter, points));
            }
        }

        private void ShuffleTiles()
        {
            int n = _tiles.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                Tile value = _tiles[k];
                _tiles[k] = _tiles[n];
                _tiles[n] = value;
            }
        }

        public void SetTilesList(List<Tile> tiles)
        {
            _tiles = tiles;
        }

        public Dictionary<string, int> GetTileCount()
        {
            return _tileCount;
        }
    }

}
