
class VariableAndParameter
{
    public void ParasmModifierInt(params int[] angka)
    {
        foreach (var n in angka)
        {
            Console.WriteLine(n);
        }
    }

    public void ParamsAndModifierString(params string[] message)
    {
        foreach (var p in message)
        {
            Console.WriteLine("Pesan: " + p);
        }
    }

    public void OptionalParameter(int angka = 3)
    {
        Console.WriteLine(angka);
    }

    public void NamedArgument(int no, string name)
    {
        Console.WriteLine(no + " " + name);
    }

    public void RefLocal()
    {
        int a = 10;
        ref int b = ref a;
        b = 30;
        Console.WriteLine(a);
    }
}