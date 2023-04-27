using System;
using Xunit;
using Wiki_Game;
using System.Linq;

namespace Wiki_Game_Test
{
    public class WikiWeb_Test
    {
        [Fact]
        public void GetHTMLFromUrl_Test()
        {
            string Url = @"https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string s = WikiHTML.GetHTMLFromUrl(Url);
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            string partOfPage = @"<a href=""/wiki/Declarative_programming"" title=""Declarative programming"">declarative</a>";
            Assert.Contains(partOfPage, s);
        }

        [Fact]
        public void ParseLinksFromHTML_Test()
        {
            string Url = @"https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string s = WikiHTML.GetHTMLFromUrl(Url);
            string[] links = WikiHTML.ParseLinksFromHTML(s).ToArray();
            Assert.NotNull(links);
            Assert.NotEmpty(links);
            Assert.Contains(@"https://en.wikipedia.org/wiki/.NET_Framework", links);
            Assert.Contains(@"https://en.wikipedia.org/wiki/Python_(programming_language)", links);
            Assert.Contains(@"https://en.wikipedia.org/wiki/Programming_paradigm", links);
            foreach (var link in links)
            {
                Assert.False(String.IsNullOrEmpty(link));
            }
        }

        [Fact]
        public void Searcher_Test()
        {
            string start = @"https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = @"https://en.wikipedia.org/wiki/Programming_paradigm";
            WikiController wc = new WikiController(start, end);
            Assert.True(wc.SetupSeach());
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited >= 0);
        }

        [Fact]
        public void Starter_1Task_Test()
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Roslyn_(compiler)";
            WikiController wc = new WikiController(start, end, 1);
            wc.StartSearch();
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited >= 1);
            Assert.Contains(end.ToLower(), wc.DstUrl);
        }

        [Fact]
        public void Starter_8Tasks_Test()
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Roslyn_(compiler)";
            WikiController wc = new WikiController(start, end, 8);
            wc.StartSearch();
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited >= 1);
            Assert.Contains(end.ToLower(), wc.DstUrl.ToLower());
        }

        //[Fact]
        //public void System_Test()
        //{
        //    string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
        //    string end = "https://en.wikipedia.org/wiki/Programming_paradigm#Support_for_multiple_paradigms";
        //    WikiController WC = new WikiController(start, end);
        //    string found = WC.Starter().GetAwaiter().GetResult();
        //    Assert.Equal(end, found);
        //}


    }
}
