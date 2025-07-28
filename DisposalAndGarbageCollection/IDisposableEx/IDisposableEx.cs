namespace DisposableAngGarbageCollection.IDisposableEx
{

    // Irreversible Disposal
    public class IrreversibleDisposal : IDisposable
    {
        private bool _disposed = false;



        public void Use()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(IrreversibleDisposal));
            }
            Console.WriteLine("memakai resource....");
        }

        public void Dispose()
        {
            if (_disposed) return;

            Console.WriteLine("Membersihkan resource");

            _disposed = true;
        }

    }

    // Chained Disposal

    public class Sedotan : IDisposable
    {
        public void Minum()
        {
            Console.WriteLine("Sedang minum menggunakan sedotan");
        }
        public void Dispose()
        {
            Console.WriteLine("Membuang sedotan");
        }
    }

    public class Botol : IDisposable
    {
        private Sedotan _sedotan = new Sedotan();
        public void Minum()
        {
            Console.WriteLine("Sedang minum menggunakan botol");
        }
        public void Dispose()
        {
            _sedotan.Dispose();
            Console.WriteLine("Botol dibuang");
        }
    }

    // Anonymous Disposal
    public class DisposableAction : IDisposable
    {
        private readonly Action _onDispose;
        private bool _disposed;

        public DisposableAction(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _onDispose();
                _disposed = true;
            }

            Console.WriteLine("Dispose dipanggil");
        }
    }

    public class MyEventProcessor
    {
        private bool _suspenEvent;

        public void Triggered()
        {
            if (!_suspenEvent)
            {
                Console.WriteLine("Event Triggered");
            }
        }

        public IDisposable SuspendEvent()
        {
            _suspenEvent = true;
            Console.WriteLine("Event suspend");

            return new DisposableAction(() =>
            {
                _suspenEvent = false;
                Console.WriteLine("Events resumed");
            });
        }
    }
    

}