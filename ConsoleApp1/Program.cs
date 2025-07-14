// var task = new Task1();
// task.Run(30);
// task.ShowTask1();

Console.WriteLine("Masukkan angka untuk menjalankan Task 1:");

string? input = Console.ReadLine();

if (int.TryParse(input, out int n))
{
    var task = new Task1();
    task.Run(n);
    task.ShowTask1();
}
else
{
    Console.WriteLine("Input tidak valid masukan angka");   
}