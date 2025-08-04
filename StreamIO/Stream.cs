
using System.Threading.Tasks.Dataflow;

public class ReadFile
{
    public void Read()
    {

        using (var fileStream = new FileStream("file.txt", FileMode.Open))
        using (var reader = new StreamReader(fileStream))
        {
            string content = reader.ReadToEnd(); // StreamReader mengubah byte ke string
            Console.WriteLine(content);
        }

    }

    public void Write()
    {
        using (FileStream fs = new FileStream("file.txt", FileMode.Create, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.WriteLine("Halo dunia!");
            writer.WriteLine("Ini ditulis lewat FileStream + StreamWriter.");
        }
    }
    public void Seek()
    {
        using (FileStream fs = new FileStream("file.txt", FileMode.Open))
        {
            if (fs.CanSeek)
            {
                fs.Seek(3, SeekOrigin.Begin); // Lompat ke byte ke-3 dari awal

                using (var reader = new StreamReader(fs))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                }
            }
        }
    }


}
