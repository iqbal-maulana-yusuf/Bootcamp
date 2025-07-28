public class GameControl
{
    private List<IPlayer> _players = new();
    private int _activePlayerIndex = 0;
    private IBoard _gameBoard;
    private ITileBag _tileBag;
    private IDictionary _dictionary;
    private GameState _currentState = GameState.NotStarted;
    private int _consecutiveSkips = 0;

    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 4;

    public event Func<Tile, char>? OnRequestBlankTileChar;
    public event Action<string>? OnDisplayMessage;
    public event Func<string, bool>? OnConfirmAction;
    public event Action<string, object>? OnGameEvent;
    public event Func<string, string>? OnGetUserInput;

    public GameControl(IDictionary dictionary, ITileBag tileBag, IBoard board)
    {
        _dictionary = dictionary;
        _tileBag = tileBag;
        _gameBoard = board;
    }

    public bool AddPlayer(IPlayer player)
    {
        if (_players.Count >= MAX_PLAYERS || !IsPlayerNameUnique(player.GetName()))
            return false;

        _players.Add(player);
        return true;
    }

    public bool RemovePlayer(string playerName)
    {
        var player = _players.Find(p => p.GetName() == playerName);
        if (player != null)
        {
            _players.Remove(player);
            return true;
        }
        return false;
    }

    public bool StartGame()
    {
        if (!ValidatePlayerCount()) return false;

        _currentState = GameState.InProgress;
        foreach (var player in _players)
        {
            RefillPlayerRack(player);
        }

        return true;
    }

    public IPlayer GetCurrentPlayer() => _players[_activePlayerIndex];
    public List<IPlayer> GetAllPlayers() => _players;

    public IPlayer? GetWinner()
    {
        if (_currentState != GameState.Ended) return null;
        return _players.OrderByDescending(p => p.GetScore()).FirstOrDefault();
    }

    public void RefillPlayerRack(IPlayer player)
    {
        int neededTiles = 7 - player.GetRack().Count;
        var drawn = DrawTilesFromBag(neededTiles);
        player.GetRack().AddRange(drawn);
    }

    public List<Tile> DrawTilesFromBag(int count) =>
        _tileBag.GetTiles().Take(count).ToList();

    private bool ValidatePlayerCount() =>
        _players.Count >= MIN_PLAYERS;

    private bool IsPlayerNameUnique(string name) =>
        !_players.Any(p => p.GetName().Equals(name, StringComparison.OrdinalIgnoreCase));
}
