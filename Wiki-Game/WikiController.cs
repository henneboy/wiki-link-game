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
        public int AmountOfPagesVisited { get; set; } = 0;
        private int AmountOfTasks { get; set; } = 0;
        public int MaxAmountOfTasks { get; set; }
        public List<Task> Tasks = new ();
//        public ILinkStorage Visited = new AList();
        public ILinkStorage Visited = new AHashSet();
        public Queue<string> Unvisited = new();

        public WikiController(string srcUrl, string dstUrl) : this(srcUrl, dstUrl, 1) { }

        public WikiController(string srcUrl, string dstUrl, int maxAmountOfTasks)
        {
            SrcUrl = srcUrl;
            DstUrl = dstUrl.ToLower();
            MaxAmountOfTasks = maxAmountOfTasks;
        }

        public async Task<string> Starter()
        {
            Unvisited.Enqueue(SrcUrl);
            while (Unvisited.Count < MaxAmountOfTasks)
            {
                if (NextInQueueIsDest())
                {
                    return Unvisited.Peek();
                }
                await Task.Run(() => SearchLink(NextLink()));
            }
            for (; AmountOfTasks < MaxAmountOfTasks; AmountOfTasks++)
            {
                if (NextInQueueIsDest())
                {
                    return Unvisited.Peek();
                }
                Tasks.Add(new Task(() => SearchLink(NextLink())));
            }
            Console.WriteLine("Trhoguhw");
            return Search();
        }

        public string Search()
        {
            Tasks.ForEach(t => t.Start());
            while (Unvisited.Peek() == null && !NextInQueueIsDest() || !NextInQueueIsDest())
            {
                Console.WriteLine(Visited.Count);
                Console.WriteLine(Unvisited.Count);
                int IdxOffinishedTask = Task.WaitAny(Tasks.ToArray());
                Tasks[IdxOffinishedTask] = Task.Run(() => SearchLink(NextLink()));
            }
            return Unvisited.Peek();
        }

        public string NextLink()
        {
            string Next = Unvisited.Dequeue();
            Visited.Add(Next.ToLower());
            return Next;
        }

        public bool NextInQueueIsDest() => Unvisited.Peek() != null && Unvisited.Peek().ToLower() == DstUrl;

        public void SearchLink(string SearchLink)
        {
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            string[] links = WikiHTML.GetLinksFromUrl(SearchLink).ToArray();
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
            AmountOfPagesVisited++;
        }
    }
}