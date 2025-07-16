using System.Dynamic;
using System.Security.Cryptography;

namespace CreatedType.Classes
{
    // modifier readonly hanya bisa di set ketika deklarasi dan menggunakan contructor saja
    public class ClassReadOnly
    {
        private readonly string Name;
        // private readonly string Name = "Iqbal"; dideklarasikan langsung

        public ClassReadOnly(string name)
        {
            Name = name;
        }

        public void Show()
        {
            Console.WriteLine(Name);
        }
    }


    public class OverLoadingConstructur
    {
        public string Nama;
        public int Umur;

        public OverLoadingConstructur(string nama)
        {
            Nama = nama;
            Umur = 0;
        }

        public OverLoadingConstructur(string nama, int umur)
        {
            Nama = nama;
            Umur = umur;
        }

        public void Show()
        {
            Console.WriteLine($"Nama : {Nama} \nUmur : {Umur}");
        }
    }

    // Non Public Constructor
    public class Logger
    {
        private static readonly Logger _instance = new Logger();

        private Logger() { }

        public static Logger GetInstace()
        {
            return _instance;
        }

        public void Log(string message)
        {
            Console.WriteLine($"Log: {message}");
        }
    }


    // Deconstructor
    public class Rectangle
    {
        public readonly int Height, Width;

        public Rectangle(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public void Deconstruct(out int height, out int width)
        {
            height = Height;
            width = Width;
        }


    }

    public class InitSetter
    {
        public string? Name { get; init; } = "Budi";
    }

    public class IndexerClass
    {
        string[] word = "iqbal maulana yusuf".Split();

        public string this[int wordNum]
        {
            get { return word[wordNum]; }
        }
    }

    public class PrimaryConstructor(string Nama, int Umur)
    {
        public string? Alamat;
        public PrimaryConstructor(string alamat) : this("iqbal", 35)
        {
            Alamat = alamat;
        }
        public void Print() => Console.WriteLine($"Nama : {Nama} \nUmur : {Umur}");

    }

    public class Server
    {
        public static string? Hostname;
        public static int Port;

        static Server()
        {
            Console.WriteLine("Static constructor di panggil");
            Hostname = "localhost:";
            Port = 8080;
        }

        public void Run()
        {
            Console.WriteLine($"Server ruuning on {Hostname}{Port}");
        }
    }

    public partial class Person
    {
        public string? FirstName;
        public string? LastName;

    }

    public partial class Person
    {
        public void sayHello()
        {
            Console.WriteLine($"Hello {FirstName} {LastName}");
        }
    }


}