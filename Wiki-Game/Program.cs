using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki_Game
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string fastEnd = "https://en.wikipedia.org/wiki/Roslyn_(compiler)";
            string mediumEnd = "https://en.wikipedia.org/wiki/Kernel_(operating_system)";
            string longEnd = "https://en.wikipedia.org/wiki/Washington_(state)";
            WikiGame.VisitPage("https://en.wikipedia.org/wiki/C_Sharp_(programming_language)", longEnd, 16);
        }
    }
}