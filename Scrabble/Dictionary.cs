public interface IDictionary
{
    HashSet<string> GetWordSet();
}

public class Dictionary : IDictionary
{
    private HashSet<string> _validWords;

    public Dictionary(string filePath)
    {
        _validWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var word = line.Trim();
                if (!string.IsNullOrEmpty(word))
                    _validWords.Add(word);
            }
        }
        else
        {
            throw new FileNotFoundException("Word bank file not found!", filePath);
        }
    }

    public HashSet<string> GetWordSet()
    {
        return _validWords;
    }
}
