using System;
using System.Diagnostics;

namespace Wiki_Game
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Programming_paradigm";
            // Without GetContentDiv
            // 12,8 sek
            // 131 pages
            Stopwatch watch = new Stopwatch();
            watch.Start();
            WikiWeb.DoSearch(start, end);
            watch.Stop();
            string Time = watch.Elapsed.TotalSeconds.ToString();
            Console.WriteLine("Traveled from: " + start + " to: " + end);
            Console.WriteLine("It took: " + Time + " seconds");
            Console.WriteLine(WikiWeb.AmountOfPagesVisited + " pages have been visited");
        }
    }
}