using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Wiki_Game
{
    public static class WikiWeb
    {
        private static Hashtable visited = new();
        const string linkStr = @"https://en.wikipedia.org/wiki/";
        public static int AmountOfPagesVisited { get; set; } = 0;
        public static int AmountOfThreads { get; set; } = 1;
        public static int MaxAmountOfThreads { get; } = 10;
        public static string DestinationPage { get; set; } = "";
        public static string FoundDestination { get; set; }

        public static void QueueSearcher(string srcUrl)
        {
            Queue<string> unvisited = new();
            unvisited.Enqueue(srcUrl);

            while (unvisited.Peek().ToLower() != DestinationPage)
            {
                if (!visited.Contains(unvisited.Peek().ToLower()))
                {
                    visited.Add(unvisited.Peek().ToLower(), unvisited.Peek().ToLower());
                    // With GetContentDiv:
                    //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
                    foreach (string link in ParseLinksFromHTML(GetHTMLFromUrl(linkStr + unvisited.Dequeue())))
                    {
                        if (AmountOfThreads < MaxAmountOfThreads)
                        {

                            unvisited.Enqueue(link);
                            //AmountOfThreads++;
                            break;
                        }
                        unvisited.Enqueue(link);
                    }
                    AmountOfPagesVisited++;
                }
                else
                {
                    unvisited.Dequeue();
                }
            }

            FoundDestination = unvisited.Peek();
            SearchComplete();
        }

        public static void SearchComplete()
        {
            Console.WriteLine("I found: " + FoundDestination);
            Console.WriteLine("Found it");
        }

        public static void SearchSetup(string srcUrl, string dstUrl)
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

        //public static void ParseHTMLForLinksAndEnqueue(string HTML)
        //{
        //    const string hrefStr = @"href=""/wiki/";
        //    int hrefIndex = 0;
        //    int quoteMark;
        //    string link;
        //    do
        //    {
        //        hrefIndex = HTML.IndexOf(hrefStr, hrefIndex);
        //        if (hrefIndex != -1)
        //        {
        //            quoteMark = HTML.IndexOf(@"""", hrefIndex + hrefStr.Length);
        //            link = HTML.Substring(hrefIndex + hrefStr.Length, quoteMark - (hrefIndex + hrefStr.Length));
        //            if (IsNotWrongType(link) && !visited.Contains(link.ToLower()))
        //            {
        //                unvisited.Enqueue(link);
        //            }
        //            hrefIndex = quoteMark;
        //        }
        //    } while (hrefIndex != -1);
        //}

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
            if (link.StartsWith("File:") || link.StartsWith("Special:") || link.StartsWith("Template:") || link.StartsWith("Category:") || link.StartsWith("Wikipedia:"))
            {
                return false;
            }
            return true;
        }
    }
}