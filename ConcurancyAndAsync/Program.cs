

using ConcurancyAndAsync.Threading;

using ConcurancyAndAsync.Threading;
var thread = new AddThread();
// thread.CreateThread();


var worker1 = new Worker("Worker-1");
var worker2 = new Worker("Worker-2");

worker1.Start();
worker2.Start();

// Thread.Sleep(1000);
worker1.CheckStatus();
worker2.CheckStatus();

// Thread.Sleep(1500); // tunggu semua thread selesai
worker1.CheckStatus();
worker2.CheckStatus();

// Console.WriteLine($"Thread utama: {Thread.CurrentThread.ManagedThreadId}");

// ---------------------------Join Thread--------------------------------

// var join = new JoinAndSleep();
// join.CreateThread();
// join.TimeOut();


// ---------------------------Safety Thread--------------------------------
// var safety = new ThreadSafe();
// safety.Main();




// ---------------------------Foreground And Background--------------------------------


// using System;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         // Foreground Thread
//         Thread foregroundThread = new Thread(() =>
//         {
//             Console.WriteLine("🔵 Foreground thread dimulai.");
//             Thread.Sleep(1000); // Simulasi pekerjaan 5 detik
//             Console.WriteLine("🔵 Foreground thread selesai.");
//         });

//         // Background Thread
//         Thread backgroundThread = new Thread(() =>
//         {
//             Console.WriteLine("🟢 Background thread dimulai.");
//             Thread.Sleep(5000); // Simulasi pekerjaan 5 detik
//             Console.WriteLine("🟢 Background thread selesai.");
//         });

//         backgroundThread.IsBackground = true; // ubah jadi background

//         // Start both threads
//         foregroundThread.Start();
//         backgroundThread.Start();

//         Console.WriteLine("🏁 Main method selesai (tunggu apa yang terjadi setelah ini)...");
//     }
// }


// ---------------------------Signaling--------------------------------

// using System;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         var signal = new ManualResetEvent(false); // false = tertutup (belum ada sinyal)

//         Thread worker = new Thread(() =>
//         {
//             Console.WriteLine("Thread worker: Menunggu sinyal...");
//             signal.WaitOne(); // Thread ini akan diam (blocked) sampai sinyal dibuka
//             Console.WriteLine("Thread worker: Menerima sinyal! Melanjutkan kerja...");
//         });

//         worker.Start();

//         Console.WriteLine("Main thread: Tidur selama 2 detik...");
//         Thread.Sleep(2000); // Simulasi kerja

//         Console.WriteLine("Main thread: Memberi sinyal...");
//         signal.Set(); // Buka sinyal → thread yang menunggu akan dilepas

//         worker.Join(); // Tunggu thread selesai
//         Console.WriteLine("Main thread: Selesai");
//     }
// }

// ---------------------------Pool--------------------------------

// using System;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         Console.WriteLine("Main Thread: Menjalankan task ke Thread Pool...\n");

//         // Kirim task ke thread pool
//         ThreadPool.QueueUserWorkItem(MyThreadPoolTask);

//         Console.WriteLine("Main Thread: Menunggu sebentar...\n");
//         Thread.Sleep(3000);

//         Console.WriteLine("Main Thread: Selesai.");
//     }

//     static void MyThreadPoolTask(object? state)
//     {
//         Console.WriteLine("Thread Pool Worker: Mulai kerja...");
//         Console.WriteLine($"Apakah ini thread pool? {Thread.CurrentThread.IsThreadPoolThread}");
//         Console.WriteLine($"Apakah ini background thread? {Thread.CurrentThread.IsBackground}");

//         // Simulasi kerja
//         Thread.Sleep(1000);

//         Console.WriteLine("Thread Pool Worker: Selesai kerja.");
//     }
// }
