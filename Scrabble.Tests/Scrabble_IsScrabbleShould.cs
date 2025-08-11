namespace Scrabble.Tests;

using NUnit.Framework;

[TestFixture]
public class Scrabble_IsScrabbleShould
{
    private GameControl? _gameControl;
    private IBoard? _board;
    private IDictionary? _dictionary;
    private ITileBag? _tileBag;

    [SetUp]
    public void Setup()
    {
        string path = "C:/Users/SE-63/Documents/Bootcamp/Scrabble/WordBank.txt";
        _board = new Board();
        _dictionary = new Dictionary(path);
        _tileBag = new TileBag();
        _gameControl = new GameControl(_dictionary, _tileBag, _board);

    }

    [Test]
    public void IsAddPlayer_Player_ReturnTrue()
    {
        var player1 = new Player("iqbal");
        var player2 = new Player("iqbal");

        var result1 = _gameControl!.AddPlayer(player1);
        var result2 = _gameControl!.AddPlayer(player2);
        Assert.That(result1, Is.True, "berhasil");
        Assert.That(result2, Is.False, "berhasil");

    }

    [Test]
    public void IsRemovePlayer_Player_ReturnTrue()
    {
        _gameControl!.AddPlayer(new Player("iqbal"));
        var result = _gameControl!.RemovePlayer("iqbal");

        Assert.That(result, Is.True, "berhasil");

    }

    [Test]

    public void IsGetCurrentPlayer_GameInProgressAndPlayersExist_ReturnsPlayer()
    {
        var player1 = new Player("iqbal");
        var player2 = new Player("maulana");
        _gameControl!.AddPlayer(player1);
        _gameControl!.AddPlayer(player2);
        _gameControl.StartGame();
        var result = _gameControl.GetCurrentPlayer();
        Assert.That(result, Is.EqualTo(player1));
    }

}

