namespace Wiki_Game
{
    public interface ISearchMekanism
    {
        string StartSearch(string SrcUrl, string DstUrl, int MaxThreadOrTask);
    }
}