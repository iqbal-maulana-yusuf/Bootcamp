namespace CreatedType.Interface
{
    public interface IUndoable
    {
        void Undo();
    }

    public class TextBox : IUndoable
    {
        void IUndoable.Undo()
        {
            Console.WriteLine("Text Box Undo");
        }
    }

    public class Dokumen : IUndoable
    {
        void IUndoable.Undo()
        {
            Console.WriteLine("Dokumen Undo");
        }
    }

    public interface IStatic
    {
        static string Cek() => "Hallo";
    }

}