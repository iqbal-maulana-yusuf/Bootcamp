// dapat mengirim lebih dari satu parameter (varidaic params)
public class ParamsAndModifier
{
    public void CetakSemua(params int[] angka)
    {
        foreach (var n in angka)
        {
            Console.WriteLine(n);
        }
    }

}