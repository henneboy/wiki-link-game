using System;
using Xunit;
using Wiki_Game;

namespace Wiki_Game_Test
{
    public class UnitTest1
    {
        [Fact]
        public void GetHTMLFromUrl_Test()
        {
            string Url = @"https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string s = WikiWeb.GetHTMLFromUrl(Url);
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            string partOfPage = @"<a href=""/wiki/Declarative_programming"" title=""Declarative programming"">declarative</a>";
            Assert.Contains(partOfPage, s);
        }

        [Fact]
        public void ParseLinksFromHTML_Test()
        {
            string Url = @"https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            string s = WikiWeb.GetHTMLFromUrl(Url);
            string[] links = WikiWeb.ParseLinksFromHTML(s);
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            string partOfPage = @"<a href=""/wiki/Declarative_programming"" title=""Declarative programming"">declarative</a>";
            Assert.Contains(partOfPage, s);
        }
    }
}
