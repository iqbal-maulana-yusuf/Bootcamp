

// string path = "WordBank.txt";
// var dictionary = new Dictionary(path);
// var words = dictionary.GetWordSet();
// string input = "banana";
// if (words.Contains(input))
// {
//     Console.WriteLine($"Kata \"{input}\" valid.");
// }

// Console.WriteLine(words.Count);


// List<ITile> initialTiles = new List<ITile>
//         {
//             new Tile('A', 1),
//             new Tile('B', 3),
//             new Tile('C', 3),
//             new Tile('D', 2),
//             new Tile('E', 1)
//         };

// TileBag tileBag = new TileBag(initialTiles);
// Console.WriteLine("Sisa tile: " + tileBag.GetRemainingCount());
// Console.WriteLine("Tile yang diambil:");
// Console.WriteLine("Sisa tile: " + tileBag.GetRemainingCount());
// var drawn = tileBag.DrawTiles(3);
// foreach (var tile in drawn)
// {
//     Console.WriteLine(tile);
// }

// Console.WriteLine("Sisa setelah ambil: " + tileBag.GetRemainingCount());
// foreach (var item in tileBag.GetTiles())
// {
//     Console.Write(item + " ");
// }
// Console.WriteLine(tileBag.GetRemainingCount());

var tileWeight = new List<ITile>();

var tileData = new (char letter, int count, int score)[]
{
    ('A', 9, 1),  ('B', 2, 3),  ('C', 2, 3),  ('D', 4, 2),
    ('E', 12, 1), ('F', 2, 4),  ('G', 3, 2),  ('H', 2, 4),
    ('I', 9, 1),  ('J', 1, 8),  ('K', 1, 5),  ('L', 4, 1),
    ('M', 2, 3),  ('N', 6, 1),  ('O', 8, 1),  ('P', 2, 3),
    ('Q', 1, 10), ('R', 6, 1),  ('S', 4, 1),  ('T', 6, 1),
    ('U', 4, 1),  ('V', 2, 4),  ('W', 2, 4),  ('X', 1, 8),
    ('Y', 2, 4),  ('Z', 1, 10), (' ', 2, 0) 
};

foreach (var (letter, count, score) in tileData)
{
    for (int i = 0; i < count; i++)
    {
        tileWeight.Add(new Tile(letter, score));
    }
}

string path = "WordBank.txt";
var dictionary = new Dictionary(path);
var board = new Board();
var tileBag = new TileBag(tileWeight);

var game = new GameControl(dictionary, tileBag, board);
var player1 = new Player("iqbal");
game.RefillPlayerRack(player1);
foreach (var tile in player1.GetRack())
{
    Console.Write(tile);
}
