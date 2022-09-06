using HtmlAgilityPack;
using URL_Crawler.Models.DataModels;
using URL_Crawler.Models.Interfaces;

namespace URL_Crawler.Models.Adapters
{
    public class HtmlWebDocument : IDocument
    {
        private readonly HtmlDocument _htmlDocument;

        public HtmlWebDocument(HtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        public List<WebImage> GetImages()
        {
            // Get image nodes, check that they have a valid source, convert to custom image object for carousel.
            // Could have made it a string list and only grabbed sources, but using an Image object makes it more extensible.
            return _htmlDocument.DocumentNode.Descendants("img")
                                .Where(f => !string.IsNullOrEmpty(f.GetAttributeValue("src", string.Empty))).ToList()
                                .ConvertAll(f => new WebImage(f.GetAttributeValue("src", string.Empty), f.GetAttributeValue("alt", string.Empty)));
        }

        public List<string> GetText()
        {
            var textList = new List<string>();

            foreach (var node in _htmlDocument.DocumentNode.SelectNodes("//text()[normalize-space()]"))
            {
                if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "script" && node.ParentNode.Name != "style")
                {
                    // Separate on spaces in text body before splitting out words.
                    var splitText = node.InnerText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    // Filter out any non-words in body of text and convert to lowercase (to merge entries).
                    var words = splitText.Where(f => f.All(char.IsLetter)).ToList().ConvertAll(d => d.ToLower());

                    textList.AddRange(words);
                }
            }

            return textList;
        }
    }
}