using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace Wiki_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            //Console.WriteLine(Web.Code(start));
            List<string> links;
            Web.Jump("https://en.wikipedia.org/wiki/Apple", "https://en.wikipedia.org/wiki/Genome");
            //links = Web.HTMLParseForLinks(Web.GetHTMLFromUrl(start));
            //foreach (string link in links)
            //{
            //    Console.WriteLine(link);
            //}
            //Console.WriteLine(links.Count);

        }
    }
    public class Web
    {
        public const string hrefStr = @"href=""/wiki";
        public const string linkStr = @"https://en.wikipedia.org/wiki";
        private static List<string> visited = new();
        private static Queue<string> unvisited = new();
        public static void Jump(string srcUrl, string dstUrl)
        {
            // Format the src and dst links
            dstUrl = dstUrl.Substring(dstUrl.IndexOf(linkStr) + linkStr.Length);
            srcUrl = srcUrl.Substring(srcUrl.IndexOf(linkStr) + linkStr.Length);
            Console.WriteLine(dstUrl);
            List<string> links;
            unvisited.Enqueue(srcUrl);
            while (unvisited.Peek() != dstUrl)
            {
                visited.Add(unvisited.Peek());
                links = HTMLParseForLinks(GetHTMLFromUrl(linkStr+unvisited.Dequeue()));
                foreach (string link in links)
                {
                    if (!visited.Contains(link) || unvisited.Contains(link))
                    {
                        unvisited.Enqueue(link);
                    }
                }
            }
            Console.WriteLine(unvisited.Peek());
            Console.WriteLine("Found it");
        }
        public static string GetHTMLFromUrl(string Url)
        {
            Console.WriteLine("Visiting: " + Url);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;
        }
        public static List<string> HTMLParseForLinks(string HTML)
        {
            List<string> links = new();
            HTML = HTMLRemoveHeaderAndFooter(HTML);
            int hrefIndex;
            int quoteMark;
            string link;
            do
            {
                hrefIndex = HTML.IndexOf(hrefStr);
                if (hrefIndex != -1)
                {
                    HTML = HTML.Substring(HTML.IndexOf(hrefStr) + hrefStr.Length);
                    quoteMark = HTML.IndexOf(@"""");
                    link = HTML.Substring(0, quoteMark);
                    if (HTML[8] != '#' && !links.Contains(link))
                    {
                        links.Add(HTML.Substring(0, quoteMark));
                    }
                    HTML = HTML.Substring(quoteMark);
                }
            } while (hrefIndex != -1);
            return links;
        }

        public static string HTMLRemoveHeaderAndFooter(string HTML)
        {
            string headerTag = @"</head>";
            string footerTag = @"<footer";
            HTML = HTML.Substring(HTML.IndexOf(headerTag) + headerTag.Length);
            HTML = HTML.Substring(0, HTML.IndexOf(footerTag));
            return HTML;
        }
    }
}
