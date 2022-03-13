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
        public int AmountOfPagesVisited { get; private set; } = 0;
        private int AmountOfTasks { get; }
        private Task<string>[] Tasks;
        //        public ILinkStorage Visited = new AList();
        public readonly ILinkStorage Visited = new AHashSet();
        private Queue<string> Unvisited = new();
        private readonly object UnvisitedLock = new object();

        public WikiController(string srcUrl, string dstUrl) : this(srcUrl, dstUrl, 1) { }

        public WikiController(string srcUrl, string dstUrl, int amountOfTasks)
        {
            SrcUrl = srcUrl;
            DstUrl = dstUrl.ToLower();
            AmountOfTasks = amountOfTasks;
            Tasks = new Task<string>[AmountOfTasks];
        }

        public string StartSearch()
        {
            Unvisited.Enqueue(SrcUrl);
            // Find enough pages, one task at a time
            while (Unvisited.Count < AmountOfTasks)
            {
                if (NextInQueueIsDest())
                {
                    return Unvisited.Peek();
                }
                SearchLink(NextLink());
            }
            return Search();
        }

        public string Search()
        {
            string result = FillTaskList();
            foreach (Task<string> t in Tasks)
            {
                t.Start();
            }
            while (result == null)
            {
                int IdxOffinishedTask = Task<string>.WaitAny(Tasks);
                result = Tasks[IdxOffinishedTask].Result;
                Tasks[IdxOffinishedTask] = Task<string>.Run(() => SearchLink(NextLink()));
            }
            return result;
        }

        public string FillTaskList()
        {
            for (short i = 0; i < AmountOfTasks; i++)
            {
                if (NextInQueueIsDest())
                {
                    return Unvisited.Peek();
                }
                Tasks[i] = new Task<string>(() => SearchLink(NextLink()));
            }
            return null;
        }

        public string NextLink()
        {
            string next;
            lock (UnvisitedLock)
            {
                next = Unvisited.Dequeue();
            }
            Visited.Add(next.ToLower());
            return next;
        }

        public bool NextInQueueIsDest(){
            lock (UnvisitedLock) {
                return Unvisited.Peek().ToLower() == DstUrl;
            }
        }

        public string SearchLink(string SearchLink)
        {
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            string[] links = WikiHTML.GetLinksFromUrl(SearchLink).ToArray();
            List<string> filteredLinks = new();
            if (links.Length != 0)
            {
                foreach (string link in links)
                {
                    if (!Visited.Contains(link.ToLower()))
                    {
                        if (link == DstUrl)
                        {
                            return link;
                        }
                        filteredLinks.Add(link);
                    }
                }
            }
            lock (UnvisitedLock)
            {
                filteredLinks.ForEach(s => Unvisited.Enqueue(s));
            }
            AmountOfPagesVisited++;
            return null;
        }
    }
}