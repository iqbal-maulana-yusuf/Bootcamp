public class BoardSquare
{
    public Tile? PlacedTile { get; set; }
    public BonusSquareType Bonus { get; set; }

    public bool IsEmpty => PlacedTile == null;

    public BoardSquare(BonusSquareType bonus = BonusSquareType.Normal)
    {
        Bonus = bonus;
    }
}
