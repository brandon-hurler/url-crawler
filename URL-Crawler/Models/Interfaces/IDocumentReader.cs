namespace URL_Crawler.Models.Interfaces
{
    public interface IDocumentReader<T> where T : class
    {
        IDocument Read(string input);
    }
}