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
            links = Web.HTMLParser(Web.Code(start));
            foreach (string link in links)
            {
                Console.WriteLine(link);
            }

        }
    }
    public class Web
    {
        private List<string> visited = new();
        private Queue<string> unvisited = new();
        public void Jump(string srcUrl, string dstUrl)
        {
            List<string> links;
            unvisited.Enqueue(srcUrl);
            while (unvisited.Peek() != dstUrl)
            {
                links = HTMLParser(Code(unvisited.Dequeue()));
                foreach (string link in links)
                {
                    if (!visited.Contains(link))
                    {
                        unvisited.Enqueue(link);
                    }
                }
            }
            Console.WriteLine("Found it");
        }
        public static string Code(string Url)
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
        public static List<string> HTMLParser(string rawHTML)
        {
            List<string> links = new();
            const string hrefStr = @"href=""/wiki";
            const string trashStart = hrefStr + @"/Special:SpecialPages";
            const string trashEnd = hrefStr + @"bc-editpage";
            // remove header
            rawHTML = rawHTML.Substring(rawHTML.IndexOf("</head>")+7);
            rawHTML = rawHTML.Substring(0, rawHTML.IndexOf("<footer"));
            int trashIdx;
            while ((trashIdx = rawHTML.IndexOf(trashStart)) != -1)
            {
                rawHTML=rawHTML.Remove(trashIdx, (rawHTML.IndexOf(trashEnd) - trashIdx));
            }
            // parse the links of the body 
            Console.WriteLine("HrefStr: " + hrefStr);
            int hrefIndex;
            while (true)
            {
                hrefIndex = -1;
                rawHTML = rawHTML.Substring(rawHTML.IndexOf(hrefStr) + hrefStr.Length);
                hrefIndex = rawHTML.IndexOf(@"""");
                if (hrefIndex != -1)
                {
                    if (rawHTML[8] != '#')
                    {
                        links.Add(rawHTML.Substring(0, hrefIndex));
                        Console.WriteLine(rawHTML.Substring(0, hrefIndex));
                    }
                    rawHTML = rawHTML.Substring(hrefIndex);
                }
                else
                {
                    break;
                }
            }
            return links;
        }
    }
}
