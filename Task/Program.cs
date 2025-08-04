
// ---------------------------Task 3------------------------------

// Console.Write("Masukkan angka untuk menjalankan Task 1: ");

// string? input = Console.ReadLine();

// if (int.TryParse(input, out int n))
// {
//     // Task2.Run(n); Task-2
//     // Task3.Run(n);
//     Task3Dict.Run(n);
// }
// else
// {
//     Console.WriteLine("Input tidak valid. Masukkan angka.");
// }


// ---------------------------Task 4------------------------------
var printer = new NumberPrinter();
printer.AddRule(3, "foo");
printer.AddRule(4, "baz");
printer.AddRule(5, "bar");
printer.AddRule(7, "jazz");
printer.AddRule(9, "huzz");
printer.Print(100);