using System.Security.Cryptography.X509Certificates;

namespace Advance.Delegate
{
    // bentuk umum delegate
    public delegate int Transformer(int x);

    public class TransformerHelper
    {
        public static int Square(int x) => x * x;
    }

    // plug-in method

    public class PluginMethod
    {
        public static int Square(int x) => x * x;
        public static int Cube(int x) => x * x * x;

        public static void Transform(int[] values, Transformer t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }
    }


    // Instance Method Target 
    public class Test
    {
        public int Square(int x) => x * x;
    }

    // Multicas Delegate

    public delegate void MessageHandler(string message);


    public class Message
    {
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void UppercaseMessage(string message)
        {
            Console.WriteLine(message.ToUpper());
        }
    }

    // Generic Delegate

    public delegate TResult TransformerGeneric<TResult, TArgs>(TArgs args);

    public class Utils
    {
        public static int Square(int x) => x * x;

        public static void Transform<T>(T[] values, TransformerGeneric<T, T> t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }
    }

    // Func and Action Delegate

    public class FuncAction
    {
        public static int Cube(int x) => x*x*x;
        public static void Transform<T>(T[] values, Func<T, T> t)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }
    }

}