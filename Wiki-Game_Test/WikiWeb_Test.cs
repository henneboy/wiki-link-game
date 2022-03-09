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
            foreach (var link in links)
            {
                Assert.NotEmpty(link);
            }
        }

        [Theory]
        [InlineData("File:")]
        [InlineData("")]
        public void ValidLink_WrongInput_Test(string link)
        {
            Assert.False(WikiHTML.ValidLink(link));
        }
        [Theory]
        [InlineData("Idk")]
        [InlineData("C_Sharp_(programming_language)")]
        public void ValidLink_ValidInput_Test(string link)
        {
            Assert.True(WikiHTML.ValidLink(link));
        }

        [Fact]
        public void Searcher_Test()
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Programming_paradigm#Support_for_multiple_paradigms";
            WikiController wc = new WikiController(start, end);
            wc.Unvisited.Enqueue(start);
            wc.SearchLink(wc.NextLink());
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited == 1);
            Assert.NotEmpty(wc.Unvisited);
        }

        [Fact]
        public void Starter_1Task_Test()
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Roslyn_(compiler)";
            WikiController wc = new WikiController(start, end, 1);
            wc.Starter().GetAwaiter().GetResult();
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited >= 1);
            Assert.NotEmpty(wc.Unvisited);
            Assert.Contains(end, wc.Unvisited);
        }

        [Fact]
        public void Starter_50Tasks_Test()
        {
            string start = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string end = "https://en.wikipedia.org/wiki/Roslyn_(compiler)";
            WikiController wc = new WikiController(start, end, 50);
            wc.Starter().GetAwaiter().GetResult();
            Assert.True(wc.Visited.Contains(start.ToLower()));
            Assert.True(wc.AmountOfPagesVisited >= 1);
            Assert.NotEmpty(wc.Unvisited);
            Assert.Contains(end, wc.Unvisited);
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
