using System.Security.Cryptography.X509Certificates;

namespace Advance.DelegateImplementation
{
    //Generic Delegate 
    public class GenericDelegate
    {
        public static void DoubleNumber<T>(T angka, Action<T> doubled)
        {
            doubled(angka);
        }

        public static void Antrian<T1, T2>(Dictionary<T1, T2> data, Action<T1, T2> antrian) where T1 : notnull
        {
            foreach (var (key, value) in data){
                antrian(key, value);
            }
        }
    }

    public class MethodPointer
    {
        public static void Doubled(int data)
        {
            Console.WriteLine(data * 2);
        }

        public static void AntrianPointer(int urutan, string nama)
        {
            Console.WriteLine($" {urutan} atas nama {nama}");
        }
    }
}