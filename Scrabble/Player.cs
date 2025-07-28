public interface IPlayer
{
    string GetName();
    int GetScore();
    List<Tile> GetRack();
    void AddScore(int points);
}

public class Player : IPlayer
{
    private string _name;
    private int _score;
    private List<Tile> _rack;

    public Player(string name)
    {
        _name = name;
        _score = 0;
        _rack = new List<Tile>();
    }

    public string GetName() => _name;
    public int GetScore() => _score;
    public List<Tile> GetRack() => _rack;
    public void AddScore(int points) => _score += points;
}

public class TileBag : ITileBag
{
    private List<Tile> _tiles;
    private Random _random = new Random();

    public TileBag()
    {
        _tiles = new List<Tile>();
        InitializeStandardTiles();
    }

    public List<Tile> GetTiles() => _tiles.OrderBy(x => _random.Next()).ToList();
    public int GetRemainingCount() => _tiles.Count;

    public void InitializeStandardTiles()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            int points = (c == 'Q' || c == 'Z') ? 10 : 1;
            for (int i = 0; i < 2; i++)
            {
                _tiles.Add(new Tile(c, points));
            }
        }
        _tiles.Add(new Tile(' ', 0)); // blank tile
    }
}
