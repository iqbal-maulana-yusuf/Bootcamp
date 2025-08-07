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


public class Mahasiswa : IPlayer
{
    public void AddScore(int points)
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public int GetScore()
    {
        throw new NotImplementedException();
    }

    public void SetName(string name)
    {
        throw new NotImplementedException();
    }

    public void SetScore(int score)
    {
        throw new NotImplementedException();
    }
}
