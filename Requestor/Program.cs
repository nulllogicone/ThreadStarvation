using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;


namespace Requestor
{
    class Program
    {
        static HttpClient http = new HttpClient();
        static string path = "hello";
        static List<Thread> requestThreads = new List<Thread>();

        // Maps long response strings to short codes for console output
        static string ShortenResponse(string input)
        {
            return input switch
            {
                "sync-over-sync" => "sos",
                "async-over-sync" => "aos",
                "sync-over-async (☠)" => "soa ☠",
                "async-over-async" => "aoa*",
                _ => input
            };
        }

        static void Main(string[] args)
        {
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0])) path = args[0];
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Hammering endpoint: /{path}");
            Console.WriteLine("Press ESC to stop, up- down- keys to change request count");
            http.Timeout = TimeSpan.FromMilliseconds(3000);

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                var k = Console.ReadKey();
                if (k.Key == ConsoleKey.UpArrow)
                {
                    AddRequestThread();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Requests: " + requestThreads.Where(t => t.IsAlive == true).Count());
                    Console.ResetColor();
                }
                if (k.Key == ConsoleKey.DownArrow)
                {
                    if (requestThreads.Count > 0)
                    {
                        requestThreads.RemoveAll(dt => dt.IsAlive == false);
                        if (requestThreads.Count == 0) continue;
                        var t = requestThreads[0];
                        t.Interrupt();
                        requestThreads.Remove(t);
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Requests: " + requestThreads.Where(t => t.IsAlive == true).Count());
                    Console.ResetColor();
                }
            }
        }

        static void RequestAndLogLoopAsync()
        {
            try
            {
                while (true)
                {
                    var sw = Stopwatch.StartNew();
                    var res = http.GetAsync($"http://localhost:5000/{path}").Result;
                    var cnt = res.Content.ReadAsStringAsync().Result;

                    Console.Write($"{sw.ElapsedMilliseconds}-{ShortenResponse(cnt)}, ");
                }
            }
            catch (ThreadInterruptedException)
            {
                requestThreads.RemoveAll(dt => dt.IsAlive == false);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + ex.StackTrace);
                Console.ResetColor();
                // put thread back after crash
                AddRequestThread();
            }
        }

        private static void AddRequestThread()
        {
            var t = new Thread(RequestAndLogLoopAsync)
            {
                IsBackground = true
            };
            t.Start();
            requestThreads.Add(t);
        }
    }
}
