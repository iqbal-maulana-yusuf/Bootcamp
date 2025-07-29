public enum MoveError
    {
        None, // Tidak ada error
        InvalidPlacement,
        WordNotInDictionary,
        NotConnected,
        InvalidFirstMove,
        TileNotInRack,
        InvalidCoordinates,
        TooFewPlayers,
        TooManyPlayers,
        DuplicatePlayerName,
        GameAlreadyStarted,
        InvalidTilesToSwap,
        TooManyTilesToSwap
    }