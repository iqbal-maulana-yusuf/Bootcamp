public class Square
{
    private int _x;
    private int _y;
    private BonusSquareType _bonusType;
    private Tile? _currentTile;
    private bool _isBonusUsed;

    public Square(int x, int y, BonusSquareType bonusType)
    {
        _x = x;
        _y = y;
        _bonusType = bonusType;
    }

    public int GetX() => _x;
    public int GetY() => _y;
    public BonusSquareType GetBonusType() => _bonusType;
    public Tile? GetTile() => _currentTile;
    public void SetTile(Tile tile) => _currentTile = tile;
    public void SetBonusUsed(bool status) => _isBonusUsed = status;
}