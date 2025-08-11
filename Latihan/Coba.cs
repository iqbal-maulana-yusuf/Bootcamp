namespace Latihan.Game
{

    public class Player
    {
        private int NIM;

        public void SetNIM(int nim)
        {
            NIM = nim;
        }

        public int GetNIM()
        {
            return NIM;
        }
    }

    public class Game
    {
        private Player _player;

        public string Nama;
        public Game(string inputName, Player player)
        {
            Nama = inputName;
            _player = player;
        }

        public void Identitas()
        {
            _player.GetNIM();
        }

    }

}

