using HtmlAgilityPack;
using URL_Crawler.Models.Interfaces;

namespace URL_Crawler.Models.Adapters
{
    public class HtmlWebDocumentReader : IDocumentReader<HtmlWebDocumentReader>
    {
        private readonly HtmlWeb _webClient;

        public HtmlWebDocumentReader()
        {
            _webClient = new HtmlWeb();
        }

        public IDocument Read(string url)
        {
            return new HtmlWebDocument(_webClient.Load(url), url);
        }
    }
}