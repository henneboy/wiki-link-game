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
            string longEnd = "https://en.wikipedia.org/wiki/Video_game";
            WikiGame.VisitPage("https://en.wikipedia.org/wiki/C_Sharp_(programming_language)", longEnd, 500);
            // https://en.wikipedia.org/wiki/Nintendo
            // https://en.wikipedia.org/wiki/Free_and_open-source_software
            // https://en.wikipedia.org/wiki/Video_game
        }
    }
}