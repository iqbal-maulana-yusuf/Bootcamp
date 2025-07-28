public interface ITileBag
{
    List<Tile> GetTiles();
    int GetRemainingCount();
}


public class Tile
{
    public char Letter { get; set; }
    public int Value { get; set; }

    public Tile(char letter, int value)
    {
        Letter = letter;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Letter}({Value})";
    }
}
