namespace Advance.TryAndCatch
{

    public class DivisionByZero
    {
        public int Calc(int x) => 10 / x;

        public void Test()
        {
            try
            {
                int y = Calc(0);
                Console.WriteLine(y);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}