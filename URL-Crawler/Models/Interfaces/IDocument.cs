using URL_Crawler.Models.DataModels;

namespace URL_Crawler.Models.Interfaces
{
    public interface IDocument
    {
        List<string> GetText();

        // Could make a generic Image interface to allow Web or File-based images to render from disk or memory stream.
        List<WebImage> GetImages();
    }
}