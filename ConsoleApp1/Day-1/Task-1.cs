
public class Task1
{
    private List<string> _data = new List<string>();
 
    public void Run(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            if ((i % 3 == 0) && (i % 5 == 0))
            {
                _data.Add("foobar");
            }
            else if (i % 3 == 0)
            {
                _data.Add("foo");
            }
            else if (i % 5 == 0)
            {
                _data.Add("bar");
            }
            else
            {
                _data.Add(i.ToString());
            }
        }
    }

    public void ShowTask1()
    {
        Console.WriteLine(string.Join(", ", _data));
    }
}