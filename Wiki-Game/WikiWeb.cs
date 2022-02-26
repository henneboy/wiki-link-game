using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Wiki_Game
{
    public class WikiWeb
    {
        public static Hashtable visited = new();
        const string linkStr = @"https://en.wikipedia.org/wiki/";
        public static int AmountOfPagesVisited { get; set; } = 0;
        public static int AmountOfTasks { get; set; } = 0;
        public static int MaxAmountOfTasks { get; } = 50;
        public static string DestinationPage { get; set; } = "";
        public static string FoundDestination { get; set; }
        public static Task[] Tasks = new Task[MaxAmountOfTasks];
        public static Queue<string> unvisited = new();

        public static void DoSearch(string srcUrl, string dstUrl)
        {
            // Format the src and dst links
            DestinationPage = dstUrl.Substring(dstUrl.IndexOf(linkStr) + linkStr.Length).ToLower();
            srcUrl = srcUrl.Substring(srcUrl.IndexOf(linkStr) + linkStr.Length);
            QueueSearcher(srcUrl);
        }

        public static async Task QueueSearcher(string srcUrl)
        {
            unvisited.Enqueue(srcUrl);
            Tasks[AmountOfTasks++] = Searcher(srcUrl);
            while (unvisited.Peek().ToLower() != DestinationPage)
            {
                Console.CursorLeft = 100;
                Console.Write(Tasks.Where(t=>t != null).Count());
                Console.CursorLeft = 0;
                if (AmountOfTasks < MaxAmountOfTasks)
                {
                    Tasks[AmountOfTasks++] = Task.Run(() => Searcher(unvisited.Dequeue()));
                    AmountOfTasks++;
                }
                int IdxOffinishedTask = Task.WaitAny(Tasks);
                Tasks[IdxOffinishedTask] = Searcher(unvisited.Dequeue());
            }
            FoundDestination = unvisited.Peek();
            SearchComplete();
        }

        public static async Task Searcher(string srcUrl)
        {
            visited.Add(srcUrl.ToLower(), unvisited.Peek().ToLower());
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            await Task.Run(()=>
            { 
                foreach (string link in WikiHTML.GetLinksFromUrl(linkStr + srcUrl))
                {
                    unvisited.Enqueue(link);
                }
            });
            AmountOfPagesVisited++;
        }

        public static void SearchComplete()
        {
            Console.WriteLine("I found: " + FoundDestination);
            Console.WriteLine("Found it");
        }
    }
}