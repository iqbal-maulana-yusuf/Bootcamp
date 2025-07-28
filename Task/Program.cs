
Console.Write("Masukkan angka untuk menjalankan Task 1: ");

string? input = Console.ReadLine();

if (int.TryParse(input, out int n))
    {
        Task1.Run(n);
    }
else
    {
            Console.WriteLine("Input tidak valid. Masukkan angka.");
    }