using System;
using System.Collections.Generic;

public class Task1
{
    public static void Run(int n)
    {
        List<string> data = new List<string>();

        for (int i = 1; i <= n; i++)
        {
            string result = "";

            if (i % 3 == 0) result += "foo";
            if (i % 5 == 0) result += "bar";
            if (i % 7 == 0) result += "jazz";

            if (result == "")
                result = i.ToString();

            data.Add(result);
        }

        Console.WriteLine(string.Join(", ", data));
    }
}
