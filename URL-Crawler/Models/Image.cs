namespace URL_Crawler.Models
{
    public class Image
    {
        public Image(string source, string altText = "")
        {
            Source = source;
            AltText = altText; // Can leave empty for decorative images only
        }

        public string Source { get; set; }

        public string AltText { get; set; }
    }
}