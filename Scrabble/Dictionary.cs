
public interface IDictionary
{
    HashSet<string> GetAllWords();
    void SetAllWords(HashSet<string> words);
    void LoadDictionary(string filePath);
}

public class Dictionary : IDictionary
{
    private HashSet<string> _allWords;

    public Dictionary(string dictionaryFilePath)
    {
        _allWords = new HashSet<string>();
        LoadDictionary(dictionaryFilePath);
    }

    public HashSet<string> GetAllWords()
    {
        return _allWords;
    }

    public void SetAllWords(HashSet<string> words)
    {
        _allWords = words;
    }

    public void LoadDictionary(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: Dictionary file not found at {filePath}");
            return;
        }

        try
        {
            foreach (string line in File.ReadLines(filePath))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _allWords.Add(line.Trim().ToUpper());
                }
            }
            Console.WriteLine($"Dictionary loaded with {_allWords.Count} words.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading dictionary: {ex.Message}");
        }
    }
}
