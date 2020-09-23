using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumbersCounter
{
    class Program
    {
        static void MainP()
        {
            //naj-amatiorskiqt nachin da si napravq asynhronnost e taka:
            //Task.Run(PrintPrimeCount); //hvani tozi method i go pusni v nov thread, a paralelno si prodyljavaj s while-a
            //noviqt thread ima za nachalen method PrintPrimeCount, tova mu e pyrviqt method v Call Stack-a!!!


            //po advance nachin da si pravq thread:

            //Thread thread = new Thread(PrintPrimeCount);
            //hvani tozi method i go pusni v nov thread, a paralelno si prodyljavaj s while-a
            //noviqt thread ima za nachalen method PrintPrimeCount, tova mu e pyrviqt method v Call Stack-a,
            //vse edno mu e Main() methoda!!!
            //Tozi method (delegate) trqbwa da NE poluchawa parameters i da NE vryshta resultat!!!! 
            //Tozi method e nachaloto na Threada!!!!!

            //moje i s Lambda izraz taka:
            //Thread thread2 = new Thread(() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Console.WriteLine(i);
            //    }
            //});

            Thread thread = new Thread(PrintPrimeCount);
            thread.Name = "1";
            Thread thread2 = new Thread(PrintPrimeCount);
            thread2.Name = "2";

            thread.Start(); //samo sled towa thread-a pochwa da raboti!!!
            //thread.Join(); //tozi spira rabotata na programata, dokato ne priklyuchi threada!!!Chaka go!!!
            thread2.Start();
            //thread2.Join();//tozi spira rabotata na programata, dokato ne priklyuchi threada!!!Chaka go!!! 

            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine(input.ToUpper());
            }

            //towa e resultata ot rabotata na 1 thread:
            //7562 = miliseconds thread1
            //664580 = count thread1
            //7576 = miliseconds thread2
            //664580 = count thread2
        }

        static void PrintPrimeCount()
        {
            Stopwatch sw = Stopwatch.StartNew();

            int n = 10000000;
            int count = 0;
            for (int i = 1; i <= n; i++)
            {
                bool isPrime = true;
                //for (int j = 2; j < i; j++)
                for (int j = 2; j <= Math.Sqrt(i); j++) //ako tyrsq do kvadraten koren bilo dostatychno vika Niki.
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    count++;
                }
            }

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(count);
        }
    }
    class ProgramWithSafetyThreading
    {
        static int Count;
        static object lockObj = new object();
        static void MainP()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Thread thread1 = new Thread(() => PrintPrimeCount(1, 2_500_000));
            thread1.Name = "1";
            Thread thread2 = new Thread(() => PrintPrimeCount(2_500_001, 5_000_000));
            thread2.Name = "2";
            Thread thread3 = new Thread(() => PrintPrimeCount(5_000_001, 7_500_000));
            thread3.Name = "3";
            Thread thread4 = new Thread(() => PrintPrimeCount(7_500_001, 10_000_000));
            thread4.Name = "4";

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(Count);

            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine(input.ToUpper());
            }

            //towa e resultata ot rabotata na 4 threada, koito smetnata po 1/4 ot cislata:
            //2711 the sum of miliseconds from all threads' work - 3 pyti po-byrzo e svyrshena 
            //obshtata rabota, otkolkoto samo s 1 thread
            //664580 = Count - Veren Count!!!!
        }

        static void PrintPrimeCount(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                bool isPrime = true;
                //for (int j = 2; j < i; j++)
                for (int j = 2; j <= Math.Sqrt(i); j++) //ako tyrsq do kvadraten koren bilo dostatychno vika Niki.
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    lock (lockObj)
                    {
                        Count++;
                    }

                    ////kak raboti lock: lock e syntaxis sugar
                    ////monitor - towa e primitiv na OS
                    ////tova e coda zad lock-a:
                    //Monitor.Enter(lockObj);
                    //Count++;
                    //Monitor.Exit(lockObj);
                }
            }
        }
    }
    class ProgramWithWrongThreading
    {
        static int Count;
        static void MainP()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Thread thread1 = new Thread(() => PrintPrimeCount(1, 2_500_000));
            Thread thread2 = new Thread(() => PrintPrimeCount(2_500_001, 5_000_000));
            Thread thread3 = new Thread(() => PrintPrimeCount(5_000_001, 7_500_000));
            Thread thread4 = new Thread(() => PrintPrimeCount(7_500_001, 10_000_000));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(Count);

            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine(input.ToUpper());
            }

            //towa e resultata ot rabotata na 4 threada, koito smetnata po 1/4 ot cislata:
            //2756 the sum of miliseconds from all threads' work - 3 pyti po-byrzo e svyrshena 
            //obshtata rabota, otkolkoto samo s 1 thread
            //660742 = Count - Greshen Count!!!!
            //Count e greshen, zashtoto za da se napravi Count++ se izpylnqwat 3 operacii pone v RAM i processora
            //a po vreme na vsqka edna ot tqh moje otdelnite threads da se namesqt paralelno i da obyrkat brojkata.
            //Naprimer - thread2 e vzel Count che e 101, promenq go na 102, no dokato minat nqkolkoto procesorni
            //instrukcii i rabota s RAM, thread1 moje da e doshyl i da e pital kolko e Count i i toj da e vzel
            //101, posle thread2 veche si e zapisal 102 v Count, no thread1 si raboti s 101 i toj syshto otiva
            //sled towa i si zapisva 102 v Count. Taka, vmesto da stane 103, Count si ostava 102 i se gubi edna
            //brojka!!!!
        }

        static void PrintPrimeCount(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                bool isPrime = true;
                //for (int j = 2; j < i; j++)
                for (int j = 2; j <= Math.Sqrt(i); j++) //ako tyrsq do kvadraten koren bilo dostatychno vika Niki.
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    Count++;
                }
            }
        }
    }
    class ProgramWIthWrongThreadingInAList
    {
        static void MainP()
        {
            List<int> numbers = Enumerable.Range(0, 10000).ToList(); //dava mi list s chislata ot 0 do 10000
            for (int i = 0; i < 4; i++)
            {
                new Thread(() =>
                {
                    while (numbers.Count > 0)
                        numbers.RemoveAt(numbers.Count - 1);
                }).Start();
            }
            //Tozi cod throwva System.ArgumentOutOfRangeException, zashtoto nqkolko nishki se borqt za posledniq element, 
            //a toj e iztrit veche ot pyrvata thread, koqto se e dokopala do nego!!!!
        }
    }
    class ProgramWItThreadingSafetyInAList
    {
        static void MainP()
        {
            List<int> numbers = Enumerable.Range(0, 10000).ToList(); //dava mi list s chislata ot 0 do 10000
            for (int i = 0; i < 4; i++)
            {
                new Thread(() =>
                {
                    while (numbers.Count > 0)
                        lock (numbers)
                        {
                            if (numbers.Count == 0) break;
                            int lastIndex = numbers.Count - 1;
                            numbers.RemoveAt(lastIndex);
                        }
                }).Start();
            }
            //Tozi cod e pravilen
        }
    }
    class ExceptionInThreadWrong
    {
        static void MainP()
        {
            try
            {
                new Thread(() => throw new Exception()).Start();
            }
            catch (Exception)
            {
            }

            //resultat:
            //cqlata programa gyrmi i spira!!!!
            //vyobshte ne hvanah greshkata!!! A daje ochakvah nishto da ne vidq na Console.....
            //Unhandled exception. System.Exception: Exception of type 'System.Exception' was thrown.
        }
    }
    class ExceptionInThreadRight
    {
        static void MainP()
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        throw new Exception();
                    }
                    catch (Exception)
                    {
                    }
                }).Start();
            }
            catch (Exception)
            {
            }

            //resultat: threada si hvana greshkata i nqmam nishto na Console.
        }
    }
    class TimeToCreate1000Threads
    {
        static void MainP()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                var tr = new Thread(() => { });
                tr.Start();
                Thread.Sleep(100);
            }
            Console.WriteLine(sw.Elapsed); //00:00:00.0929020 - tova ne e mnogo vreme, no ako
            //tezi threadove stoqt i nishto ne pravqt, togawa OS shte se zatormozi ujasno mnogo ot towa da
            //gi poddyrja!!! Osven towa OS se natovari na 100% za da gi syzdade!!!!
        }
    }
    class Tasks
    {
        static void MainP()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {

                    }
                });
            }
            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();
            //ako go spra v debug pri i==20, to ovijdam, che za 20 taska, Task Scheduler mi e 
            //napravil 8 threada. 8 threada e napravil, zashtoto imam 8 virtualni logical 
            //processors na moqta mashina i toj e napravil po 1 thread za wseki ot tqh, za da 
            //moje da si puska rabota po tezi 8 nishki!!!
        }
    }
    class DownloadingPureVic
    {
        static void MainP()
        {
            Stopwatch sw = Stopwatch.StartNew();

            HttpClient httpClient = new HttpClient();

            for (int i = 0; i <= 100; i++)
            {
                var url = $"https://vicove.com/vic-{i}";

                var httpResponse = httpClient.GetAsync(url).GetAwaiter().GetResult();

                var vic = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Console.WriteLine(vic.Length);
            }
            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();

            //za 6 secundi mi svali tezi 100 stranici bez threads.
        }
    }
    class DownloadingVicWithThreads
    {
        static void MainP()
        {
            Stopwatch sw = Stopwatch.StartNew();

            HttpClient httpClient = new HttpClient();

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i <= 100; i++)
            {
                Thread thread = new Thread(() =>
                {
                    var url = $"https://vicove.com/vic-{i}";
                    var httpResponse = httpClient.GetAsync(url).GetAwaiter().GetResult();
                    var vic = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Console.WriteLine(vic.Length);
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();

            //za 6 secundi mi svali tezi 100 stranici bez threads.
            //za 4.3 secundi se svaliha tezi 100 stranici s threads.
        }
    }
    class DownloadingVicWithTasks
    {
        static void MainP()
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i <= 100; i++)
            {
                string url = $"https://vicove.com/vic-{i}";
                //Task task = Task.Run(async () =>
                //{
                //    HttpClient httpClient = new HttpClient();
                //    var httpResponse = await httpClient.GetAsync(url); //tova e zadacha, koqto trqbwa da se chaka
                //    //da se svyrshi, no tq zavisi ot vynshen resource!!!
                //    var vic = await httpResponse.Content.ReadAsStringAsync();//tova e zadacha, koqto trqbwa da se chaka
                //    //da se svyrshi, no tq zavisi ot vynshen resource!!!
                //    Console.WriteLine(vic.Length);
                //});

                //moga i taka da go vikna Task-a - towa e analogichno na gornoto:
                Task task = Task.Run(async () =>
                {
                    await DownloadAsync(url);
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray()); //chakam vsichki da svyrshtat, predi da prodylja nadolu.

            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();

            //za 6 secundi mi svali tezi 101 stranici bez threads.
            //za 4.3 secundi se svaliha tezi 101 stranici s threads.
            //za 4.3 s. pyrviqt pyt i za 1.8 s. vtoriqt pyt se svaliha tezi 101 stranici s tasks.
        }

        static async Task DownloadAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            //var token = new CancellationToken();
            var httpResponse = await httpClient.GetAsync(url/*, token*/);
            //     A cancellation token that can be used by other objects or threads to receive
            //     notice of cancellation.
            var vic = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(vic.Length);
        }
    }
    class DownloadingVicWithTasksBeforeAsyncAndAwait
    {
        static void Main()
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i <= 100; i++)
            {
                string url = $"https://vicove.com/vic-{i}";
                Task task = Task.Run(async () => { await DownloadAsync(url); });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray()); //chakam vsichki da svyrshtat, predi da prodylja nadolu.

            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();

            //za 6 secundi mi svali tezi 101 stranici bez threads.
            //za 4.3 secundi se svaliha tezi 101 stranici s threads.
            //za 1.6 s. se svaliha tezi 101 stranici s tasks s methoda now.
            //za 3.5 s. se svaliha tezi 101 stranici s tasks s methoda before

        }

        ////before:
        //static async Task DownloadAsync(string url)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    httpClient.GetAsync(url).ContinueWith((httpResponse) =>
        //     {
        //         httpResponse.Result.Content.ReadAsStringAsync().ContinueWith((vic) =>
        //         {
        //             Console.WriteLine(vic.Result.Length);
        //         });
        //     });

        //    //vsichko sled ContinueWith(....) se naricha promise, t.e. promise e method, kojto mu
        //    //podawam, kato tozi method shte se izpylni, sled kato predhodniqt e gotov. Tuk:
        //    //sled kato se izpylni GetAsync(url), az mu obeshtvam, dawam mu nov method, po kojto
        //    //da raboti - httpResponse.Result.Content.ReadAsStringAsync(), a sled kato posledniqt
        //    //se izpylni, az mu dawam treti method, kojto da se izpylni - Console.WriteLine(vic.Result.Length)
        //}

        //now:
        static async Task DownloadAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(url);
            var vic = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(vic.Length);
        }
    }

}
