using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Wiki_Game
{
    public static class WikiHTML
    {
        public static IEnumerable<string> GetLinksFromUrl(string Url) => ParseLinksFromHTML(GetHTMLFromUrl(Url));


        public static string GetHTMLFromUrl(string Url)
        {
            Console.CursorLeft = 0;
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
                    if (ValidLink(link))
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

        public static bool ValidLink(string link)
        {
            if (link.StartsWith("File:") || link.StartsWith("Special:") || link.StartsWith("Template:") || link.StartsWith("Category:") || link.StartsWith("Wikipedia:") || string.IsNullOrEmpty(link))
            {
                return false;
            }
            return true;
        }
    }
}