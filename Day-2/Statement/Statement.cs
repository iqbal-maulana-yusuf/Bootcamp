
public class StatementType
{
    public void VariableDeclarations()
    {
        string name = "iqbal";
        int umur = 25;
        Console.WriteLine($"nama : {name} \numur : {umur}");
    }
    public void ConstantDeclarations()
    {
        const float pi = 3.14F;
        Console.WriteLine($"nilai pi adalah {pi}");
    }
    public void SwitchStatement(int n)
    {
        switch (n)
        {
            case 1:
                Console.WriteLine("A");
                break;
            case 2:
                Console.WriteLine("B");
                break;
            default:
                Console.WriteLine("None");
                break;
        }
    }


}