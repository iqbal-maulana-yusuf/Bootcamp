using System.Threading.Channels;

namespace Advance.EnumeratorAndIterator
{
    // foraeach manual
    public class ForeachManual
    {
        public List<string> Buah = ["apel", "jeruk", "mangga"];
        public IEnumerator<string> penunjuk;

        public ForeachManual()
        {
            penunjuk = Buah.GetEnumerator();
        }

        public void ShowBuah()
        {
            while (penunjuk.MoveNext())
            {
                Console.WriteLine(penunjuk.Current);
            }
        }
    }
    public class ForeachManualDict
    {
        public Dictionary<string, int> Data = new Dictionary<string, int> {
            {"iqbal",1},
            {"maulana",2}
        };
        public IEnumerator<KeyValuePair<string, int>> penunjuk;

        public ForeachManualDict()
        {
            penunjuk = Data.GetEnumerator();
        }

        public void ShowBuah()
        {
            while (penunjuk.MoveNext())
            {
                Console.WriteLine(penunjuk.Current);
            }
        }
    }

    // Producing Sequences with yield
    public class ProduceSequence
    {
        public int x;
        public IEnumerable<int> Print()
        {
            for (int i = 0; i < x; i++)
            {
                yield return i;
            }
        }


        public void Iterate()
        {
            foreach (var x in Print())
            {
                Console.Write(x + " ");
            }
        }
    }

    // composing sequence
    public class ComposingSequence
    {
        public int x;

        public IEnumerable<int> GetNumber()
        {
            for (int i = 0; i < x; i++)
            {
                yield return i;
            }
        }

        public IEnumerable<int> DoubelNumber(IEnumerable<int> number)
        {
            foreach (var x in number)
            {
                yield return x * 2;
            }
        }

        public void Iterate(IEnumerable<int> doubleNumber)
        {
            foreach (var y in doubleNumber)
            {
                Console.Write(y + " ");
            }
        }
    }

    

}