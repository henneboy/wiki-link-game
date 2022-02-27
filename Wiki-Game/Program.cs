using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki_Game
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Programming_paradigm#Support_for_multiple_paradigms";
            // Without GetContentDiv
            // 12,8 sek
            // 131 pages
            Stopwatch watch = new Stopwatch();
            watch.Start();
            WikiController WC = new WikiController(start, end);
            string found = WC.Starter().GetAwaiter().GetResult(); 
            //string found = WikiWeb.DoSearch(start, end).Result;
            watch.Stop();
            string Time = watch.Elapsed.TotalSeconds.ToString();
            Console.WriteLine("Traveled from: " + start + " to: " + end);
            Console.WriteLine("The found site was: " + found);
            Console.WriteLine("It took: " + Time + " seconds");
            Console.WriteLine(WC.AmountOfPagesVisited + " pages have been visited");
            //Console.WriteLine("items");
            //string[] s = WikiWeb.ParseLinksFromHTML(WikiWeb.GetHTMLFromUrl(start)).ToArray();
            //foreach (var item in s)
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}