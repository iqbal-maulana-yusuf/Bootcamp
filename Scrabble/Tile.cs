public class Tile
{
    private char _letter;
    private int _points;

    public Tile(char letter, int points)
    {
        _letter = char.ToUpper(letter); // Pastikan huruf besar
        _points = points;
    }

    public char GetLetter()
    {
        return _letter;
    }

    public int GetPoints()
    {
        return _points;
    }

    public void SetLetter(char letter)
    {
        _letter = char.ToUpper(letter);
    }

    public void SetPoints(int points)
    {
        _points = points;
    }

    public override string ToString()
    {
        return $"{_letter} ({_points})";
    }
}
