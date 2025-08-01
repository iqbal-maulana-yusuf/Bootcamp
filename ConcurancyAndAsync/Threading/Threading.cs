namespace ConcurancyAndAsync.Threading
{
    public class AddThread
    {
        public void CreateThread()
        {
            Thread t = new Thread(WriteY);

            t.Start();

            for (var i = 0; i < 10; i++) Console.Write("X");
        }
        public void WriteY()
        {
            for (var i = 0; i < 10; i++) Console.Write("Y");
        }
    }
    public class Worker
    {
        private Thread _thread;
        private string _name;

        public Worker(string name)
        {
            _name = name;
            _thread = new Thread(DoWork);
            _thread.Name = name; // kasih nama ke thread
        }

        public void Start()
        {
            _thread.Start();
        }

        public void CheckStatus()
        {
            Console.WriteLine($"Thread {_name} IsAlive? {_thread.IsAlive}");
        }

        private void DoWork()
        {
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Memulai pekerjaan...");
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(i);
            }
            Console.WriteLine($"[{Thread.CurrentThread.Name}] Pekerjaan selesai.");
        }
    }

    public class JoinAndSleep
    {
        public void CreateThread()
        {
            var t = new Thread(Go);
            t.Start();
            // t.Join();

            Console.WriteLine("Menunggu selesai");
        }

        public void TimeOut()
        {
            var t = new Thread(Go);
            t.Start();
            bool selesai = t.Join(1500);

            if (selesai)
            {
                Console.WriteLine("eksekusi selesai tepat waktu");
            }
            else
            {
                Console.WriteLine("eksekusi lambat, timeout!");
            }

        }

        public void Go()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Eksekusi Method GO Selesai");
        }
    }
    // Thread Safety

    public class ThreadSafe
    {
        static bool _done;
        static readonly object _locker = new object(); // A dedicated object for locking

        public void Main()
        {
            new Thread(Go).Start();
            Go();
        }

        public void Go()
        {
            lock (_locker) // Only one thread can enter this block at a time for _locker
            {
                if (!_done) { Console.WriteLine("Done"); _done = true; }
            }

        }
    }
}