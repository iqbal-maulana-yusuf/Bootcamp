

using ConcurancyAndAsync.Threading;

// using ConcurancyAndAsync.Threading;
// var thread = new AddThread();
// // thread.CreateThread();


// var worker1 = new Worker("Worker-1");
// var worker2 = new Worker("Worker-2");

// worker1.Start();
// worker2.Start();

// Thread.Sleep(1000);
// worker1.CheckStatus();
// worker2.CheckStatus();

// Thread.Sleep(1500); // tunggu semua thread selesai
// worker1.CheckStatus();
// worker2.CheckStatus();

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


// ---------------------------Delegete Thread--------------------------------
// class Program
// {
//     static void Main()
//     {
//         ThreadStart task = new ThreadStart(Method1);
//         task += Method2;
//         task += Method3;

//         Thread t = new Thread(task);
//         t.Start();
//     }

//     static void Method1()
//     {
//         Console.WriteLine("Method1 dijalankan oleh thread: " + Thread.CurrentThread.ManagedThreadId);
//     }

//     static void Method2()
//     {
//         Console.WriteLine("Method2 dijalankan oleh thread: " + Thread.CurrentThread.ManagedThreadId);
//     }

//     static void Method3()
//     {
//         Console.WriteLine("Method3 dijalankan oleh thread: " + Thread.CurrentThread.ManagedThreadId);
//     }
// }


// ---------------------------Lambda Expression Thread--------------------------------
using System;
using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         Thread t = new Thread(() =>
//         {
//             Console.WriteLine("Thread dijalankan dengan lambda.");
//             for (int i = 1; i <= 5; i++)
//             {
//                 Console.WriteLine($"Angka: {i}");
//                 Thread.Sleep(500);
//             }
//         });

//         t.Start();
//         Console.ReadLine(); // Supaya console tidak langsung tertutup
//     }
// }

// ---------------------------Race Condition With Locking--------------------------------

// var locking = new object();
// for (int i = 0; i < 10; i++)
// {
//     lock (locking)
//         new Thread(() => Console.WriteLine(i)).Start();
// }

// ---------------------------Captured Variable--------------------------------

// int x = 5;
// var t = new Thread(() => Console.WriteLine(x));
// x = 20;
// t.Start();


// using System;
// using System.Threading;

// class Program
// {
//     static readonly object lock1 = new object();
//     static readonly object lock2 = new object();

//     static void Main()
//     {
//         Thread t1 = new Thread(DeadlockMethod1);
//         Thread t2 = new Thread(DeadlockMethod2);

//         t1.Start();
//         t2.Start();

//         // t1.Join(); // tunggu sampai selesai
//         // t2.Join();
//     }

//     static void DeadlockMethod1()
//     {
//         lock (lock1)
//         {
//             Console.WriteLine("Thread 1: lock1 didapat");
//             Thread.Sleep(1000);
//             Console.WriteLine("Thread 1: mencoba lock2...");
//             lock (lock2)
//             {
//                 Console.WriteLine("Thread 1: lock2 didapat");
//             }
//         }
//     }

//     static void DeadlockMethod2()
//     {
//         lock (lock2)
//         {
//             Console.WriteLine("Thread 2: lock2 didapat");
//             Thread.Sleep(1000);
//             Console.WriteLine("Thread 2: mencoba lock1...");
//             lock (lock1)
//             {
//                 Console.WriteLine("Thread 2: lock1 didapat");
//             }
//         }
//     }
// }


// using System;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         using (Mutex mutex = new Mutex(false, "MutexDemo"))
//         {
//             // Tunggu maksimal 5 detik untuk mencoba ambil kunci
//             if (!mutex.WaitOne(5000, false))
//             {
//                 Console.WriteLine("Aplikasi sudah dijalankan di tempat lain.");
//                 return; // Keluar kalau mutex sudah dipegang oleh proses lain
//             }

//             Console.WriteLine("Aplikasi sedang berjalan...");
//             Console.ReadKey(); // Simulasi aplikasi aktif
//         }
//     }
// }

// ---------------------------Semapohore--------------------------------

// using System;
// using System.Threading;

// class Program
// {
//     // Inisialisasi semaphore: 2 thread maksimum yang bisa masuk bersamaan
//     static Semaphore semaphore = new Semaphore(2, 2, "SemaphoreDemo");

//     static void Main()
//     {
//         for (int i = 1; i <= 5; i++)
//         {
//             Thread t = new Thread(AccessResource);
//             t.Name = $"Thread {i}";
//             t.Start();
//         }
//     }

//     static void AccessResource()
//     {
//         Console.WriteLine($"{Thread.CurrentThread.Name} menunggu giliran...");
//         semaphore.WaitOne(); // coba masuk (ambil slot)

//         Console.WriteLine($"{Thread.CurrentThread.Name} masuk ke area kritis.");
//         Thread.Sleep(2000); // simulasi kerja

//         Console.WriteLine($"{Thread.CurrentThread.Name} keluar.");
//         semaphore.Release(); // lepas slot
//     }
// }


// ---------------------------Semapohore--------------------------------


// using System;
// using System.Threading;

// class Program
// {
//     static AutoResetEvent autoEvent = new AutoResetEvent(false); // kondisi awal: tidak ada sinyal

//     static void Main()
//     {
//         Thread worker = new Thread(WorkerMethod);
//         worker.Start();

//         Thread.Sleep(2000); // simulasi kerja lain, biarkan worker menunggu

//         Console.WriteLine("Main thread memberi sinyal...");
//         autoEvent.Set(); // kirim sinyal agar thread lain bisa lanjut

//         worker.Join(); // tunggu sampai worker selesai
//         Console.WriteLine("Main thread selesai.");
//     }

//     static void WorkerMethod()
//     {
//         Console.WriteLine("Worker: Menunggu sinyal...");
//         autoEvent.WaitOne(); // thread ini menunggu sinyal

//         Console.WriteLine("Worker: Menerima sinyal, melanjutkan kerja...");
//         Thread.Sleep(1000);
//         Console.WriteLine("Worker: Selesai.");
//     }
// }

// using System;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         for (int i = 0; i < 10; i++)
//         {
//             ThreadPool.QueueUserWorkItem(new WaitCallback(MyMethod));
//         }

//         Console.ReadLine();
//     }

//     static void MyMethod(object? state)
//     {
//         Thread current = Thread.CurrentThread;
//         Console.WriteLine($"Thread Pool: {current.IsThreadPoolThread}, ID: {current.ManagedThreadId}");
//     }
// }

// ---------------------------TASK--------------------------------


// using System;
// using System.Threading;
// using System.Threading.Tasks;



// class Program
// {
//     static async Task Main()
//     {
//         var tasks = new Task[10];

//         for (int i = 0; i < 10; i++)
//         {
//             tasks[i] = Task.Run(() =>
//             {
//                 Thread t = Thread.CurrentThread;
//                 Console.WriteLine($"Thread Pool: {t.IsThreadPoolThread}, ID: {t.ManagedThreadId}");
//             });
//         }

//         await Task.WhenAll(tasks); // tunggu semua selesai
//         Console.WriteLine("Semua tugas selesai.");
//     }
// }








// var task = Task.Run(() => Console.WriteLine("Foo"));
// task.Wait(); // tunggu sampai task selesai

// ---------------------------Task Status--------------------------------


// var task = Task.Run(() => Console.WriteLine("Doing work..."));

// Console.WriteLine(task.Status); // bisa tampil Running
// task.Wait();
// Console.WriteLine(task.Status); // sekarang RanToCompletion

// ---------------------------Task Return Value--------------------------------

// Task<int> task = Task.Run(() =>
// {
//     Console.WriteLine("Menghitung...");
//     return 3 + 2;
// });

// // ... bisa melakukan hal lain di sini ...

// int hasil = task.Result; // Tunggu hingga task selesai, lalu ambil nilainya
// Console.WriteLine(hasil); // Output: 5


// async Task<int> TambahAsync()
// {
//     await Task.Delay(1000); // simulasi kerja
//     return 10 + 20;
// }

// int hasil = await TambahAsync(); // tidak blokir thread
// Console.WriteLine(hasil);

// ---------------------------Task Exception--------------------------------
// Task task = Task.Run(() =>
// {
//     throw new NullReferenceException("Oops!");
// });

// try
// {
//     task.Wait(); // Menunggu task selesai → jika error, exception dilempar di sini
// }
// catch (AggregateException aex)
// {
//     // Semua exception task dibungkus dalam AggregateException
//     if (aex.InnerException is NullReferenceException)
//     {
//         Console.WriteLine(aex.Message);
//     }
// }


// Task t = Task.Run(() => throw new InvalidOperationException());

// await t.ContinueWith(task =>
// {
//     if (task.IsFaulted)
//     {
//         Console.WriteLine("Terjadi error: " + task.Exception?.InnerException?.Message);
//     }
// });

// using System;
// using System.Threading.Tasks;

// class Program
// {
//     static void Main()
//     {
//         Task<int> task = Task.Run(() =>
//         {
//             // Hitung angka
//             return 2 + 3;
//         });

//         var awaiter = task.GetAwaiter();
//         awaiter.OnCompleted(() =>
//         {
//             int result = awaiter.GetResult(); // Dapatkan hasilnya
//             Console.WriteLine($"Hasilnya: {result}");
//         });

//         // Agar aplikasi tidak langsung selesai
//     }
// }


// ---------------------------TaskCompletionSource--------------------------------


// using System;
// using System.Threading.Tasks;
// using System.Threading;

// class Program
// {
//     static Task<int> BuatTaskDenganDelay()
//     {
//         var tcs = new TaskCompletionSource<int>();

//         // Timer manual: tunggu 3 detik lalu selesaikan task
//         Task.Run(() =>
//         {
//             Thread.Sleep(3000);           // Tunggu 3 detik
//             tcs.SetResult(100);           // Selesaikan task dengan hasil 100
//         });

//         return tcs.Task; // Kembalikan task-nya
//     }

//     static async Task Main()
//     {
//         Console.WriteLine("Menunggu hasil...");
//         int hasil = await BuatTaskDenganDelay(); // Tunggu task selesai
//         Console.WriteLine($"Hasilnya: {hasil}");
//     }
// }


// using System;
// using System.Threading.Tasks;

// class Program
// {
//     static async Task Main()
//     {
//         Console.WriteLine("Tunggu 3 detik...");
//         await Task.Delay(3000); // Tunggu 3 detik secara async
//         Console.WriteLine("3 detik selesai!");
//     }
// }


// async Task Foo(CancellationToken token)
// {
//     for (int i = 0; i < 10; i++)
//     {
//         Console.WriteLine(i);
//         await Task.Delay(1000, token); // Delay bisa dibatalkan
//     }
// }


// var source = new CancellationTokenSource();
// var token = source.Token;

// var task = Foo(token);
// source.Cancel();



// async Task<int> Delay1() { await Task.Delay(1000); return 1; }
// async Task<int> Delay2() { await Task.Delay(2000); return 2; }
// async Task<int> Delay3() { await Task.Delay(3000); return 3; }

// // var first = await Task.WhenAny(Delay1(), Delay2(), Delay3());
// // Console.WriteLine(await first); // Output: 1

// var result = await Task.WhenAll(Delay1(), Delay2(), Delay3());
// Console.WriteLine("Semua selesai: " + string.Join(", ", result));


using System;
using System.Threading;
using System.Threading.Tasks;

// class Program
// {
//     static async Task Main()
//     {
//         // 1. Buat sumber token pembatalan
//         CancellationTokenSource cts = new CancellationTokenSource();

//         // 2. Jalankan task yang bisa dibatalkan
//         var task = CountAsync(cts.Token);

//         Console.WriteLine("Tekan ENTER untuk membatalkan...");
//         Console.ReadLine(); // Tunggu user tekan enter

//         // 3. Minta pembatalan
//         cts.Cancel();

//         try
//         {
//             await task;
//         }
//         catch (OperationCanceledException)
//         {
//             Console.WriteLine("Task dibatalkan!");
//         }
//     }

//     static async Task CountAsync(CancellationToken token)
//     {
//         for (int i = 1; i <= 10; i++)
//         {
//             token.ThrowIfCancellationRequested(); // Cek apakah diminta cancel
//             Console.WriteLine($"Menghitung: {i}");
//             await Task.Delay(1000); // Delay 1 detik
//         }

//         Console.WriteLine("Selesai menghitung!");
//     }
// }


using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Buat progress handler, akan dipanggil dari task
        var progress = new Progress<int>(percent =>
        {
            Console.Write($"Progress: {percent}%");
        });

        await DoWorkAsync(progress);

        Console.WriteLine("Selesai!");
    }

    static async Task DoWorkAsync(IProgress<int> progress)
    {
        // Simulasi kerja 10 langkah
        for (int i = 1; i <= 10; i++)
        {
            await Task.Delay(500); // Simulasi proses
            progress.Report(i * 10); // Laporkan progres (10%, 20%, dst.)
        }
    }
}
