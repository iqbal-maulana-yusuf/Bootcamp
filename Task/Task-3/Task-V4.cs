using System;
using System.Collections.Generic;

public class Task3Dict
{
    public static void Run(int n)
    {
        // Mapping angka ke string output-nya
        Dictionary<int, string> rules = new Dictionary<int, string>
        {
            { 3, "foo" },
            { 4, "bazz" },
            { 5, "bar" },
            { 7, "jazz" },
            { 9, "huzz" }
        };

        List<string> data = new List<string>();

        for (int i = 1; i <= n; i++)
        {
            string result = "";

            foreach (var rule in rules)
            {
                if (i % rule.Key == 0)
                {
                    result += rule.Value;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = i.ToString();
            }

            data.Add(result);
        }

        Console.WriteLine(string.Join(", ", data));
    }
}
