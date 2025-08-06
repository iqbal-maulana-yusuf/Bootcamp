using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardScrabble.Controller
{
    public interface IPlayer
    {
        string GetName();
        int GetScore();
        void SetName(string name);
        void AddScore(int points);
        void SetScore(int score);
    }



    public class Player : IPlayer
    {
        private string _name;
        private int _score;

        public Player(string name)
        {
            _name = name;
            _score = 0;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetScore()
        {
            return _score;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void AddScore(int points)
        {
            _score += points;
        }

        public void SetScore(int score)
        {
            _score = score;
        }
    }

}
