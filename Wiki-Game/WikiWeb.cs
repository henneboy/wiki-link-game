using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Wiki_Game
{
    public static class WikiWeb
    {
        private static Hashtable visited = new();
        private static Queue<string> unvisited = new();

        public static void Jump(string srcUrl, string dstUrl, out int nrOfPagesVisited)
        {
            nrOfPagesVisited = 0;
            // Format the src and dst links
            const string linkStr = @"https://en.wikipedia.org/wiki/";
            dstUrl = dstUrl.Substring(dstUrl.IndexOf(linkStr) + linkStr.Length).ToLower();
            srcUrl = srcUrl.Substring(srcUrl.IndexOf(linkStr) + linkStr.Length);
            unvisited.Enqueue(srcUrl);
            while (unvisited.Peek().ToLower() != dstUrl)
            {
                if (!visited.Contains(unvisited.Peek().ToLower()))
                {
                    visited.Add(unvisited.Peek().ToLower(), unvisited.Peek().ToLower());
                    // With GetContentDiv:
                    //ParseHTMLForLinksAndEnqueue(GetContentDiv(GetHTMLFromUrl(linkStr + unvisited.Dequeue())));
                    ParseHTMLForLinksAndEnqueue(GetHTMLFromUrl(linkStr + unvisited.Dequeue()));
                    nrOfPagesVisited++;
                } else
                {
                    unvisited.Dequeue();
                }
            }
            Console.WriteLine(unvisited.Peek());
            Console.WriteLine("Found it");
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

        public static void ParseHTMLForLinksAndEnqueue(string HTML)
        {
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
                        unvisited.Enqueue(link);
                    }
                    hrefIndex = quoteMark;
                }
            } while (hrefIndex != -1);
        }

        public static ICollection<string> ParseLinksFromHTML(string HTML)
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