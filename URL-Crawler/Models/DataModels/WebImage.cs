namespace URL_Crawler.Models.DataModels
{
    public class WebImage
    {
        public WebImage(string source, string altText = "")
        {
            Source = source;
            AltText = altText; // Can leave empty for decorative images only
        }

        public string Source { get; set; }

        public string AltText { get; set; }
    }
}