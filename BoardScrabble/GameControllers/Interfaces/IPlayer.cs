using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.GameControllers.Interfaces
{
    public interface IPlayer
    {
        string GetName();
        int GetScore();
        void SetName(string name);
        void AddScore(int points);
        void SetScore(int score);
    }
}
