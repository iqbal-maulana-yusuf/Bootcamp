namespace CreatedType.Struct
{
    public class Point
    {
        public int A, B;

        public Point(int a, int b)
        {
            A = a;
            B = b;
        }
    }

    public struct PointStruct
    {
        public int A, B;

        public PointStruct(int a, int b)
        {
            A = a;
            B = b;
        }
    }

    public interface IPrint
    {
        public void Run();
    }

    public class Dokumen : IPrint
    {
        public void Run()
        {
            throw new NotImplementedException();
        }
    }


}