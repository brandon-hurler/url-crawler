using URL_Crawler.Models.DataModels;
using URL_Crawler.Models.Interfaces;

namespace URL_Crawler.Models.ViewModels
{
    public class ContentViewModel
    {
        public const int BaseCount = 10;

        public ContentViewModel(IDocument document, int? wordCount = BaseCount)
        {
            var allText = document.GetText();

            Images = document.GetImages();
            TopWords = GetTopWords(allText, wordCount);
            TotalWordCount = allText.Count;
        }

        public int TotalWordCount { get; set; }

        public List<IGrouping<string, string>> TopWords { get; set; } = new List<IGrouping<string, string>>();

        public List<WebImage> Images { get; set; } = new List<WebImage>();

        private static List<IGrouping<string, string>> GetTopWords(List<string> text, int? wordCount = BaseCount)
        {
            // Prevents needing to cast below, adding additional check with fallback default.
            var count = wordCount ?? BaseCount;
            var sortedWords = text.GroupBy(f => f).OrderByDescending(f => f.Count()).ToList();

            return count < sortedWords.Count ? sortedWords.Take(count).ToList() : sortedWords;
        }
    }
}