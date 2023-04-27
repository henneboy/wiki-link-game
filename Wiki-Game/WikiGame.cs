using System;
using System.Diagnostics;

namespace Wiki_Game
{
    public static class WikiGame
    {
        /// <summary>
        /// Searches through wiki for from the startpage to the endpages and displays information about the search.
        /// </summary>
        /// <param name="startpage"></param>
        /// <param name="endpage"></param>
        /// <param name="AmountOfTasks">How many tasks may be used for the search (1 if multithreading is wanted)</param>
        /// <returns>The amount of time (in seconds) it took to find the endpage</returns>
        public static double FindPathOfLinks(string startpage, string endpage, int AmountOfTasks)
        {
            WikiController wc = new(startpage, endpage, AmountOfTasks);
            Stopwatch watch = new();
            watch.Start();
            wc.StartSearch();
            watch.Stop();
            string time = watch.Elapsed.TotalSeconds.ToString();
            Console.WriteLine($"Found page: {wc.DstUrl} from start-page: {startpage}");
            Console.WriteLine($"Time elapsed(sec): {time}, amount of tasks: {AmountOfTasks}");
            Console.WriteLine($"Amount of pages visited: {wc.Visited.Count}");
            Console.WriteLine($"Average amount of pages per second: {wc.Visited.Count / watch.Elapsed.TotalSeconds}");
            Console.WriteLine("The exact url of the destination which was found:" + wc.FoundPageURL);
            return watch.Elapsed.Seconds;
        }
    }
}