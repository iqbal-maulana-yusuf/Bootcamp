using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
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
}
