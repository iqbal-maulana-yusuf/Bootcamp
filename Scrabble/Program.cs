
string path = "WordBank.txt";
var board = new Board();
var tileBag = new TileBag();
var dictionary = new Dictionary(path);

var player1 = new Player("iqbal");
var player2 = new Player("maulana");

var game = new GameControl(dictionary, tileBag, board);

game.OnDisplayMessage += Console.WriteLine;

game.AddPlayer(player1);
game.AddPlayer(player2);

game.StartGame();