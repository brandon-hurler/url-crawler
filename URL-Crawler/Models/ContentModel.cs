using HtmlAgilityPack;

namespace URL_Crawler.Models
{
    public class ContentModel
    {
        public ContentModel(HtmlDocument htmlDocument, int wordCount)
        {
            var allText = GetAllText(htmlDocument);

            TotalWordCount = allText.Count;
            TopWords = GetTopWords(allText, wordCount);
            Images = GetImages(htmlDocument);
        }

        public int TotalWordCount { get; set; }

        public List<IGrouping<string, string>> TopWords { get; set; } = new List<IGrouping<string, string>>();

        public List<Image> Images { get; set; } = new List<Image>();

        private static List<IGrouping<string, string>> GetTopWords(List<string> text, int count = 10)
        {
            var sortedWords = text.GroupBy(f => f).OrderByDescending(f => f.Count()).ToList();

            return count < sortedWords.Count ? sortedWords.Take(count).ToList() : sortedWords;
        }

        private static List<string> GetAllText(HtmlDocument htmlDocument)
        {
            var textList = new List<string>();

            foreach (var node in htmlDocument.DocumentNode.SelectSingleNode("//body").DescendantsAndSelf())
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

        private static List<Image> GetImages(HtmlDocument htmlDocument)
        {
            // Get image nodes, check that they have a valid source, convert to custom image object for carousel.
            // Could have made it a string list and only grabbed sources, but using an Image object makes it more extensible.
            return htmlDocument.DocumentNode.Descendants("img")
                               .Where(f => !string.IsNullOrEmpty(f.GetAttributeValue("src", string.Empty))).ToList()
                               .ConvertAll(f => new Image(f.GetAttributeValue("src", string.Empty), f.GetAttributeValue("alt", string.Empty)));
        }
    }
}