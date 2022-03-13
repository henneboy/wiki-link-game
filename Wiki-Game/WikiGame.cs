using System;
using System.Diagnostics;

namespace Wiki_Game
{
    public static class WikiGame
    {
        public static double VisitPage(string startpage, string endpage, int AmountOfTasks)
        {
            WikiController wc = new(startpage, endpage, AmountOfTasks);
            Stopwatch watch = new();
            watch.Start();
            string res = wc.StartSearch();
            watch.Stop();
            string time = watch.Elapsed.TotalSeconds.ToString();
            Console.WriteLine($"Found: {res} from the page {startpage}");
            Console.WriteLine($"Time elapsed: {time}, amount of tasks: {AmountOfTasks}");
            Console.WriteLine($"Amount of pages visited: {wc.Visited.Count}");
            return watch.Elapsed.Seconds;
        }
    }
}