using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki_Game
{
    public class WikiController
    {
        public string SrcUrl { get; }
        public string DstUrl { get; }
        public string FoundPageURL { get; set; }
        public int AmountOfPagesVisited { get; private set; } = 1;
        private int AmountOfTasks { get; }
        private readonly Task<bool>[] Tasks;
        public readonly ILinkStorage Visited = new AHashSet(); // My own class, so I could see the difference between hashset and list.
        private readonly Queue<string> Unvisited = new();
        private readonly object UnvisitedLock = new();

        public WikiController(string srcUrl, string dstUrl) : this(srcUrl, dstUrl, 1) { }

        public WikiController(string srcUrl, string dstUrl, int amountOfTasks)
        {
            SrcUrl = srcUrl;
            DstUrl = dstUrl.ToLower();
            AmountOfTasks = amountOfTasks;
            Tasks = new Task<bool>[AmountOfTasks];
        }

        /// <summary>
        /// Starts the search for the destionation, this is the only property which should be public.
        /// </summary>
        /// <returns>True when the destination is found</returns>
        public bool StartSearch()
        {
            if (SetupSeach()) { return true; }
            if (FillTaskList()) { return true; }
            return DoMultiTaskSearch();
		}

        /// <summary>
        /// Find enough pages to start tasks
        /// </summary>
        /// <returns>True, if the destination is found</returns>
        public bool SetupSeach()
        {
            Unvisited.Enqueue(SrcUrl);
            while (Unvisited.Count <= AmountOfTasks)
            {
                if (SearchLink(GetNextLink()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fill the "Tasks" taskarray with tasks
        /// </summary>
        /// <returns>True if the destination is found</returns>
        public bool FillTaskList()
        {
            for (short i = 0; i < AmountOfTasks; i++)
            {
                Tasks[i] = new Task<bool>(() => SearchLink(GetNextLink()));
            }
            return false;
        }

        /// <summary>
        /// Search for the destination using multiple tasks
        /// </summary>
        /// <returns>True when the destination is found</returns>
        public bool DoMultiTaskSearch()
        {
            bool result = false;
            // Start all tasks
            foreach (Task<bool> t in Tasks)
            {
                t.Start();
            }
            while (!result)
            {
                int IdxOffinishedTask = Task<bool>.WaitAny(Tasks);
                result = Tasks[IdxOffinishedTask].Result;
                Tasks[IdxOffinishedTask] = Task<bool>.Run(() => SearchLink(GetNextLink()));
            }
            // Dispose of all tasks
            Task<bool>.WaitAll(Tasks);
            foreach (Task<bool> t in Tasks)
            {
                t.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Dequeues a link and adds it to the list of visited links
        /// </summary>
        /// <returns>The next link</returns>
        public string GetNextLink()
        {
            string next;
            lock (UnvisitedLock)
            {
                next = Unvisited.Dequeue();
            }
            Visited.Add(next.ToLower());
            return next;
        }

        /// <summary>
        /// Visits a link and adds them to the queue.
        /// </summary>
        /// <param name="SearchLink"></param>
        /// <returns>True if the destination is found, otherwise false</returns>
        public bool SearchLink(string SearchLink)
        {
            // With GetContentDiv:
            //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
            string[] links = WikiHTML.GetOutgoingLinksFromLink(SearchLink).ToArray();
            AmountOfPagesVisited++;
            List<string> unvisitedLinks = new();
            if (links.Length == 0)
            {
                return false;
            }
            foreach (string link in links)
            {
                if (Visited.Contains(link.ToLower()))
                {
                    continue;
                }
                if (link.ToLower() == DstUrl)
                {
                    FoundPageURL = link;
                    return true;
                }
                unvisitedLinks.Add(link);
            }
            lock (UnvisitedLock)
            {
                unvisitedLinks.ForEach(s => Unvisited.Enqueue(s));
            }
            return false;
        }
    }
}