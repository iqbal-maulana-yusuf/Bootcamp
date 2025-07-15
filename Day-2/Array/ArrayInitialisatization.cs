

public class ArrayInit
{
    public void ArrayInitial()
    {
        char[] vowels1 = { 'a', 'e', 'i', 'o', 'u' };
        char[] vowels2 = ['a', 'e', 'i', 'o', 'u'];
        Console.WriteLine(vowels1);
        Console.WriteLine(vowels2);

    }

    public void DeafultInit(int n)
    {
        int[] data = new int[n];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i;
        }
        Console.WriteLine(string.Join(", ", data));
    }

    public void IndicesBackward(int n)
    {
        int[] data = new int[n];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i * 10;
        }
        Console.WriteLine(string.Join(", ", data));
        Console.WriteLine(data[^1]);
        Console.WriteLine(data[^2]);

    }

    public void ArrayRange(int n, int start, int end)
    {
        int[] data = new int[n];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i * 10;
        }
        int[] bagian = data[start..end];
        Console.WriteLine("data :" + string.Join(", ", data));
        Console.WriteLine($"item dari index {start} sampai {end}: {string.Join(", ", bagian)}");
    }

    public void ArrayRangeLast(int n, int start)
    {
        int[] data = new int[n];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i * 10;
        }
        int[] bagian = data[^start..];
        Console.WriteLine("data :" + string.Join(", ", data));
        Console.WriteLine($"item dari {start} terakhir: {string.Join(", ", bagian)}");
    }
}