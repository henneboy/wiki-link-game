namespace Wiki_Game
{
    public interface ILinkStorage
    {
        void Add(string link);
        bool Contains(string link);
        public int Count { get; }
    }
}