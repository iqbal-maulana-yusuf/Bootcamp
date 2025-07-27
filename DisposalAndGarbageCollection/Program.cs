
using DisposableAngGarbageCollection.Finalizer;
using DisposableAngGarbageCollection.IDisposableEx;

// -----------------------DISPOSABLE---------------------

// ---------------------Irreversible Disposal-----------------------


// var irreversibleDisposal = new IrreversibleDisposal();
// irreversibleDisposal.Use();
// irreversibleDisposal.Dispose();
// irreversibleDisposal.Use();

// ---------------------Chained Disposal----------------------- 

// var botol = new Botol();
// botol.Minum();
// botol.Dispose();

// ---------------------Anonymous Event----------------------- 

// var processor = new MyEventProcessor();
// processor.Triggered();
// using (processor.SuspendEvent())
// {
//     processor.Triggered();
// }
// processor.Triggered();;

// using System;

// class Program
// {
//     static void Main()
//     {
//         Console.WriteLine("Sebelum alokasi objek:");
//         PrintGCCounts();

//         // GEN 0: alokasi banyak objek kecil
//         for (int i = 0; i < 10000; i++)
//         {
//             var temp = new byte[1000]; // cepat menjadi sampah
//         }

//         Console.WriteLine("\nSetelah alokasi objek sementara:");
//         PrintGCCounts();

//         // GEN 1: buat objek yang dipertahankan
//         var gen1Obj = new byte[1024 * 100]; // 100KB
//         GC.Collect(); // Trigger GC
//         GC.WaitForPendingFinalizers();

//         Console.WriteLine("\nSetelah GC pertama (harusnya gen1Obj naik ke Gen1):");
//         PrintGCCounts();
//         Console.WriteLine($"Generasi gen1Obj: {GC.GetGeneration(gen1Obj)}");

//         // GEN 2: buat objek besar yang bertahan lebih lama
//         var gen2Obj = new byte[1024 * 1024 * 10]; // 10MB
//         GC.Collect(); // GC kedua
//         GC.WaitForPendingFinalizers();

//         GC.Collect(); // GC ketiga
//         GC.WaitForPendingFinalizers();

//         Console.WriteLine("\nSetelah GC kedua dan ketiga (harusnya gen2Obj di Gen2):");
//         PrintGCCounts();
//         Console.WriteLine($"Generasi gen2Obj: {GC.GetGeneration(gen2Obj)}");

//         // Buat objek baru, seharusnya tetap di Gen 0
//         var gen0Obj = new object();
//         Console.WriteLine($"\nGenerasi gen0Obj: {GC.GetGeneration(gen0Obj)}");
//     }

//     static void PrintGCCounts()
//     {
//         Console.WriteLine($"GC Gen 0 Count: {GC.CollectionCount(0)}");
//         Console.WriteLine($"GC Gen 1 Count: {GC.CollectionCount(1)}");
//         Console.WriteLine($"GC Gen 2 Count: {GC.CollectionCount(2)}");
//     }
// }


// ---------------------FINALIZER----------------------- 

CreateObject.Create();

Console.WriteLine("Memanggil GC.Collect...");
GC.Collect();  // Meminta GC untuk membersihkan memori
GC.WaitForPendingFinalizers(); // Tunggu finalizer selesai

Console.WriteLine("Program selesai.");
Console.ReadLine(); // Supaya kamu bisa lihat output