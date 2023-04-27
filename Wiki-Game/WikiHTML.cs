using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Wiki_Game
{
    public static class WikiHTML
    {
        public static readonly string hrefStr = @"href=""/wiki/";
        public static readonly string WikiLinkPrefix = @"https://en.wikipedia.org/wiki/";

        public static IEnumerable<string> GetOutgoingLinksFromLink(string Url) => ParseLinksFromHTML(GetHTMLFromUrl(Url));

        public static string GetHTMLFromUrl(string Url)
        {
            Console.WriteLine("Reading: " + Url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string HTML = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                HTML = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return HTML;
        }

        public static IEnumerable<string> ParseLinksFromHTML(string HTML)
        {
            HashSet<string> links = new();
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
                    if (!(link.StartsWith("File:") || 
                        link.StartsWith("Special:") || 
                        link.StartsWith("Template:") || 
                        link.StartsWith("Category:") || 
                        link.StartsWith("Wikipedia:") || 
                        string.IsNullOrEmpty(link)))
                    {
                        links.Add(WikiLinkPrefix + link);
                    }
                    hrefIndex = quoteMark;
                }
            } while (hrefIndex != -1);
            return links;
        }

        /// <summary>
        /// Filters out sidebars and the bottom of the page
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetContentDiv(string HTML)
        {
            const string contentStartTag = @"<div id=""mw-content-text";
            //const string contentEndTag = @"<div id=""mw-navigation";
            const string contentEndTag = @"<noscript>";
            int startIdx = HTML.IndexOf(contentStartTag) + contentStartTag.Length;
            int endIdx = HTML.IndexOf(contentEndTag, startIdx);
            return HTML[startIdx..endIdx];
        }
    }
}