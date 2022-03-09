using System;
using System.Collections.Generic;

namespace Wiki_Game
{
    public class AHashSet : ILinkStorage
    {
        private readonly HashSet<string> Links = new();

        public void Add(string link) => Links.Add(link);

        public bool Contains(string link) => Links.Contains(link);

        public int Count => Links.Count;
    }
}