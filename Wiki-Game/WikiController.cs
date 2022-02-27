using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Wiki_Game
{
    public class WikiController
    {
        public static readonly string linkStr = @"https://en.wikipedia.org/wiki/";
        public int AmountOfPagesVisited { get; set; } = 0;
        public int AmountOfTasks { get; set; } = 0;
        public int MaxAmountOfTasks { get; } = 50;
        public List<Task> Tasks = new ();
        public Hashtable Visited = new();
        public Queue<string> Unvisited = new();

        public WikiController(string srcUrl, string dstUrl)
        {
            // Format the src and dst links
            DstUrl = dstUrl.Substring(dstUrl.IndexOf(linkStr) + linkStr.Length).ToLower();
            SrcUrl = srcUrl.Substring(srcUrl.IndexOf(linkStr) + linkStr.Length);
        }

        public async Task<string> Starter()
        {
            return await StartSearch();
        }

        public async Task<string> StartSearch()
        {
            Unvisited.Enqueue(SrcUrl);
            Tasks.Add(Searcher());
            await Tasks[AmountOfTasks];
            await Task.Run(() =>
            {
                while (Unvisited.Peek().ToLower() != DstUrl)
                {
                    Console.CursorLeft = 100;
                    Console.Write(Tasks.Count);
                    if (AmountOfTasks < MaxAmountOfTasks)
                    {
                        Tasks.Add(Task.Run(() => Searcher()));
                        AmountOfTasks++;
                    }
                    int IdxOffinishedTask = Task.WaitAny(Tasks.ToArray());
                    Tasks[IdxOffinishedTask] = Searcher();
                }
            });
            return Unvisited.Peek();
        }

        public async Task Searcher()
        {
            Visited.Add(Unvisited.Peek().ToLower(), Unvisited.Peek().ToLower());
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            await Task.Run(() =>
            {
                foreach (string link in WikiHTML.GetLinksFromUrl(Unvisited.Dequeue().ToLower()))
                {
                    if (!Visited.Contains(link.ToLower()))
                    {
                        Unvisited.Enqueue(link);
                    }
                }
            });
            AmountOfPagesVisited++;
        }

        public string SrcUrl { get; }
        public string DstUrl { get; }
    }
}