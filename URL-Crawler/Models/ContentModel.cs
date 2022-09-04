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
        }

        public List<Uri> Images { get; set; } = new List<Uri>();

        public int TotalWordCount { get; set; }

        public List<IGrouping<string, string>> TopWords { get; set; } = new List<IGrouping<string, string>>();

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
                    // Separate on spaces in text body.
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