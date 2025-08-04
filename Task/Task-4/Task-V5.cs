using System;
using System.Collections.Generic;
using System.Text;

public class NumberPrinter
{
    private readonly Dictionary<int, string> rules = new Dictionary<int, string>();

    public void AddRule(int number, string output)
    {
        rules[number] = output;
    }

    public void Print(int n)
    {
        List<string> results = new List<string>();

        for (int i = 1; i <= n; i++)
        {
            StringBuilder result = new StringBuilder();

            foreach (var rule in rules)
            {
                if (i % rule.Key == 0)
                {
                    result.Append(rule.Value);
                }
            }

            if (result.Length == 0)
                result.Append(i.ToString());

            results.Add(result.ToString());
        }

        Console.WriteLine(string.Join(", ", results));
    }
}
