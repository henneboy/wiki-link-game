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
            string[] links = WikiWeb.ParseLinksFromHTML(s).ToArray();
            Assert.NotNull(links);
            Assert.NotEmpty(links);
            Assert.Contains(".NET_Framework", links);
            Assert.Contains("Python_(programming_language)", links);
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
            Assert.False(WikiWeb.ValidLink(link));
        }
        [Theory]
        [InlineData("Idk")]
        [InlineData("C_Sharp_(programming_language)")]
        public void ValidLink_ValidInput_Test(string link)
        {
            Assert.True(WikiWeb.ValidLink(link));
        }
    }
}
