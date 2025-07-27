namespace DisposableAngGarbageCollection.Finalizer
{
    public class FinalizerDemo
    {
        private int _id;

        public FinalizerDemo(int id)
        {
            _id = id;
            Console.WriteLine($"Object {_id} dibuat");
        }

        ~FinalizerDemo()
        {
            Console.WriteLine($"Finalizer dipanggil untuk object {_id}");
        }

    }

    public class CreateObject
    {
        public static void Create()
        {
            FinalizerDemo obj1 = new FinalizerDemo(1);
            FinalizerDemo obj2 = new FinalizerDemo(2);
            FinalizerDemo obj3 = new FinalizerDemo(3);
        }
    }

    // Dispose + Finalizer

    public class MyResource : IDisposable
    {
        private bool _disposed = false;
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) return;

            if (disposing)
            {

            }

            _disposed = true;
        }

        ~MyResource() => Dispose(false);
    }
}