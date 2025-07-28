


Console.Write("Masukkan angka untuk menjalankan Task 1: ");

string? input = Console.ReadLine();

if (int.TryParse(input, out int n))
{
    // Task2.Run(n); Task-2
    // Task3.Run(n);
    Task3Dict.Run(n);
}
else
{
    Console.WriteLine("Input tidak valid. Masukkan angka.");
}