using System;
using System.Collections.Generic;

public class Task3
{
    public static void Run(int n)
    {
        List<string> data = new List<string>();

        for (int i = 1; i <= n; i++)
        {
            string result = "";

            if (i % 3 == 0) result += "foo";
            if (i % 4 == 0) result += "bazz";
            if (i % 5 == 0) result += "bar";
            if (i % 7 == 0) result += "jazz";
            if (i % 9 == 0) result += "huzz";

            if (result == "")
                result = i.ToString();

            data.Add(result);
        }

        Console.WriteLine(string.Join(", ", data));
    }
}
