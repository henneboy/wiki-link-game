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
        public string SrcUrl { get; }
        public string DstUrl { get; }
        public static readonly string linkStr = @"https://en.wikipedia.org/wiki/";
        public int AmountOfPagesVisited { get; set; } = 0;
        private int AmountOfTasks { get; set; } = 0;
        public int MaxAmountOfTasks { get; } = 50;
        public List<Task> Tasks = new ();
//        public ILinkStorage Visited = new AList();
        public ILinkStorage Visited = new AHashSet();
        public Queue<string> Unvisited = new();

        public WikiController(string srcUrl, string dstUrl)
        {
            DstUrl = dstUrl.ToLower();
            SrcUrl = srcUrl;
        }

        public async Task<string> Starter()
        {
            await SearchSetup();
            return await StartSearch();
        }

        public async Task SearchSetup()
        {
            Unvisited.Enqueue(SrcUrl);
            Tasks.Add(Task.Run(()=>Searcher(NextLink())));
            await Tasks.First();
        }

        public async Task<string> StartSearch()
        {
            await Task.Run(() =>
            {
                while (Unvisited.Peek().ToLower() != DstUrl)
                {
                    Console.WriteLine(Visited.Count);
                    Console.WriteLine(Unvisited.Count);
                    //while (true)
                    //{
                    //    if (AmountOfTasks < MaxAmountOfTasks && Unvisited.Count != 0)
                    //    {
                    //        Tasks.Add(Task.Run(() => Searcher(NextLink())));
                    //        AmountOfTasks++;
                    //    }
                    //    if (AmountOfTasks >= MaxAmountOfTasks)
                    //    {
                    //        break;
                    //    }
                    //}
                    int IdxOffinishedTask = Task.WaitAny(Tasks.ToArray());
                    Tasks[IdxOffinishedTask] = Task.Run(() => Searcher(NextLink()));
                }
            });
            return Unvisited.Peek();
        }

        public string NextLink()
        {
            string Next = Unvisited.Dequeue();
            Visited.Add(Next.ToLower());
            return Next;
        }

        public async Task Searcher(string link)
        {
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            await Task.Run(() =>
            {
                string[] links = WikiHTML.GetLinksFromUrl(link).ToArray();
                if (links.Length != 0)
                {
                    foreach (string link in links)
                    {
                        if (!Visited.Contains(link.ToLower()))
                        {
                            Unvisited.Enqueue(link);
                        }
                    }
                }
            });
            AmountOfPagesVisited++;
        }
    }
}