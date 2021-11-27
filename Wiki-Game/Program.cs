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
            string link = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            Console.WriteLine(Web.Code(link));
            List<string> links;
        }
    }
    public class Web
    {
        private List<string> visited = new();
        private Queue<string> unvisited = new();
        public void Jump(string srcUrl, string dstUrl)
        {
            List<string> links;
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
        }
        public static string Code(string Url)
        {
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
            return links;
        }
    }
}
