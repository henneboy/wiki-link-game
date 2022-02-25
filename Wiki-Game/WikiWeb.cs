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
        private static Hashtable visited = new();
        const string linkStr = @"https://en.wikipedia.org/wiki/";
        public static int AmountOfPagesVisited { get; set; } = 0;
        public static int AmountOfTasks { get; set; } = 1;
        public static int MaxAmountOfTasks { get; } = 50;
        public static string DestinationPage { get; set; } = "";
        public static string FoundDestination { get; set; }
        public static List<Task> Tasks = new();
        public static Queue<string> unvisited = new();

        public static async Task QueueSearcher(string srcUrl)
        {
            unvisited.Enqueue(srcUrl);
            Tasks.Add(Searcher(srcUrl));
            while (unvisited.Peek().ToLower() != DestinationPage)
            {
                Console.CursorLeft = 100;
                Console.Write(Tasks.Count);
                Console.CursorLeft = 0;
                if (AmountOfTasks < MaxAmountOfTasks)
                {
                    Tasks.Add(Task.Run(() => Searcher(unvisited.Dequeue())));
                    AmountOfTasks++;
                }
                Task finishedTask = await Task.WhenAny(Tasks);
                if (finishedTask != null)
                {
                    Tasks.Remove(finishedTask);
                    Tasks.Add(Searcher(unvisited.Dequeue()));
                }
            }
            Tasks.ForEach(t => t.Dispose());
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
                foreach (string link in ParseLinksFromHTML(GetHTMLFromUrl(linkStr + srcUrl)))
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

        public static void DoSearch(string srcUrl, string dstUrl)
        {
            // Format the src and dst links
            DestinationPage = dstUrl.Substring(dstUrl.IndexOf(linkStr) + linkStr.Length).ToLower();
            srcUrl = srcUrl.Substring(srcUrl.IndexOf(linkStr) + linkStr.Length);
            QueueSearcher(srcUrl);
        }

        public static string GetHTMLFromUrl(string Url)
        {
            Console.WriteLine("Reading: " + Url);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;
        }

        public static IEnumerable<string> ParseLinksFromHTML(string HTML)
        {
            ICollection<string> links = new List<string>();
            const string hrefStr = @"href=""/wiki/";
            int hrefIndex = 0;
            int quoteMark;
            string link;
            do
            {
                hrefIndex = HTML.IndexOf(hrefStr, hrefIndex);
                if (hrefIndex != -1)
                {
                    quoteMark = HTML.IndexOf(@"""", hrefIndex + hrefStr.Length);
                    link = HTML.Substring(hrefIndex + hrefStr.Length, quoteMark - (hrefIndex + hrefStr.Length));
                    if (IsNotWrongType(link) && !visited.Contains(link.ToLower()))
                    {
                        links.Add(link);
                    }
                    hrefIndex = quoteMark;
                }
            } while (hrefIndex != -1);
            return links;
        }

        public static string GetContentDiv(string HTML)
        {
            const string contentStartTag = @"<div id=""mw-content-text";
            //const string contentEndTag = @"<div id=""mw-navigation";
            const string contentEndTag = @"<noscript>";
            int startIdx = HTML.IndexOf(contentStartTag) + contentStartTag.Length;
            int endIdx = HTML.IndexOf(contentEndTag, startIdx);
            HTML = HTML.Substring(startIdx, endIdx - startIdx);
            return HTML;
        }
        public static bool IsNotWrongType(string link)
        {
            if (link.StartsWith("File:") || link.StartsWith("Special:") || link.StartsWith("Template:") || link.StartsWith("Category:") || link.StartsWith("Wikipedia:") || string.IsNullOrEmpty(link))
            {
                return false;
            }
            return true;
        }
    }
}