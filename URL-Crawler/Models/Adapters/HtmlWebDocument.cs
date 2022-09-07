using HtmlAgilityPack;
using URL_Crawler.Models.DataModels;
using URL_Crawler.Models.Interfaces;
using URL_Crawler.Extensions;
using System.Text.RegularExpressions;

namespace URL_Crawler.Models.Adapters
{
    public class HtmlWebDocument : IDocument
    {
        private readonly HtmlDocument _htmlDocument;

        public HtmlWebDocument(HtmlDocument htmlDocument, string url = "")
        {
            _htmlDocument = htmlDocument;

            SourceUrl = url;
        }

        public string SourceUrl { get; private set; }

        public List<WebImage> GetImages()
        {
            // Get image nodes, check that they have a valid source, convert to custom image object for carousel.
            // Could have made it a string list and only grabbed sources, but using an Image object makes it more extensible.
            var documentImages = _htmlDocument.DocumentNode.Descendants("img")
                                              .Where(f => !string.IsNullOrEmpty(f.GetAttributeValue("src", string.Empty)))
                                              .Select(f => new WebImage(f.GetAttributeValue("src", string.Empty), f.GetAttributeValue("alt", string.Empty)));

            var fullyQualifiedDomain = SourceUrl.GetFullyQualifiedDomain();

            // Relative image paths on domain
            var prependedImages = documentImages.Where(f => f.Source.StartsWith("/")).Select(f => new WebImage(fullyQualifiedDomain + f.Source, f.AltText));

            // Fully qualified image paths on domain
            var fullyQualifiedImages = documentImages.Where(f => !f.Source.StartsWith("/"));

            return fullyQualifiedImages.Concat(prependedImages).ToList();
        }

        public List<string> GetText()
        {
            var textList = new List<string>();

            foreach (var node in _htmlDocument.DocumentNode.SelectNodes("//text()[normalize-space()]"))
            {
                if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "script" && node.ParentNode.Name != "style")
                {
                    // Matches one or more whitespace characters and punctuation, ignoring punctuation mid-word.
                    var pattern = new Regex("\\s+|(?=\\W\\p{P}|\\p{P}\\W)|(?<=\\W\\p{P}|\\p{P}\\W})");

                    // Separate on spaces in text body before splitting out words.
                    var splitText = pattern.Split(node.InnerText).Where(f => f != string.Empty);

                    // Filter out any non-words in body of text and convert to lowercase (to merge entries).
                    var words = splitText.Where(f => f.All(char.IsLetter)).ToList().ConvertAll(d => d.ToLower());

                    textList.AddRange(words);
                }
            }

            return textList;
        }
    }
}