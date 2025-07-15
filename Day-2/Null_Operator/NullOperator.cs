public class NullOpr
{
    public void Coalescing()
    {
        string? firstname = null;
        string name = firstname ?? "masukan nama anda";
        Console.WriteLine(name);
        Console.WriteLine(firstname);
    }

    public void CoalescingAssignment()
    {
        string? firstname = null;
        string name = firstname ??= "masukan nama anda";
        Console.WriteLine(name);
        Console.WriteLine(firstname);
    }
}